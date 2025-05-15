using ASP.NETCore_WithoutEF.App.Models;
using ASP.NETCore_WithoutEF.App.DataAccess.Repository.Contract;
using ASP.NETCore_WithoutEF.App.DataAccess.Repository.Implementation;
using ASP.NETCore_WithoutEF.App.Utility;
using ASP.NETCore_WithoutEF.App.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Stripe.Checkout;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace ASP.NETCore_WithoutEF.Areas.Customer.Controllers
{
    [Area("Customer")]    
    public class CartController : Controller
    {

        private readonly IApplicationUserRepository<ApplicationUser> _userRepo;
        private readonly IOrderHeaderRepository<OrderHeader> _orderHeaderRepo;
        private readonly IProductRepository<Product> _productRepo;
        private readonly IShoppingCartRepository<ShoppingCart> _cartRepo;
        private readonly IOrderDetailRepository<OrderDetail> _orderDetailRepo;
        //string userId = "";

        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public CartController(
            IProductRepository<Product> productRepo,
            IShoppingCartRepository<ShoppingCart> cartRepo,
            IApplicationUserRepository<ApplicationUser> userRepo,
            IOrderHeaderRepository<OrderHeader> orderHeaderRepo,
            IOrderDetailRepository<OrderDetail> orderDetailRepo
            )

        {
            _productRepo = productRepo;
            _cartRepo = cartRepo;
            _userRepo = userRepo;
            _orderHeaderRepo = orderHeaderRepo;
            _orderDetailRepo = orderDetailRepo;
        }

        public async Task<IActionResult> Index()
        {
            string userId = HttpContext.Session.GetString("UserId");          

            if (userId == null)
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });               
            }
          
            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { success = false, message = "User ID not found." });
            }

            ShoppingCartVM = new()
            {
                ShoppingCartList = await _cartRepo.GetCartList(userId),
                OrderHeader = new()

            };
            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                ShoppingCartVM.OrderHeader.OrderTotal += cart.Price * cart.Count;
            }
            return View(ShoppingCartVM);
        }

        public async Task<IActionResult> Summary()
        {
            string userId = HttpContext.Session.GetString("UserId");

            ShoppingCartVM = new ShoppingCartVM()
            {
                ShoppingCartList = await _cartRepo.GetCartList(userId),
                OrderHeader = new()
            };

            ShoppingCartVM.OrderHeader.ApplicationUser = await _userRepo.GetUser(userId);

            ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
            ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.ApplicationUser.StreetAddress;
            ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
            ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.ApplicationUser.State;
            ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.ApplicationUser.PostalCode;
            ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;

            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                ShoppingCartVM.OrderHeader.OrderTotal += cart.Price * cart.Count;
            }

            return View(ShoppingCartVM);
        }
       

        [HttpPost]
        [ActionName("Summary")]
        public async Task<IActionResult> SummaryPost()
        {

            string userId = HttpContext.Session.GetString("UserId");

            ShoppingCartVM.ShoppingCartList = await _cartRepo.GetCartList(userId);

            ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
            ShoppingCartVM.OrderHeader.ShippingDate = DateTime.Now;
            ShoppingCartVM.OrderHeader.PaymentDate = DateTime.Now;
            ShoppingCartVM.OrderHeader.PaymentDueDate = DateTime.Now;


            ShoppingCartVM.OrderHeader.ApplicationUserId = userId;
            ApplicationUser applicationUser = await _userRepo.GetUser(userId);

            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                ShoppingCartVM.OrderHeader.OrderTotal += cart.Price * cart.Count;
            }

            if (applicationUser.CompanyId.GetValueOrDefault() == 0)
            {
                //it is a regular customer 
                ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
                ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
            }
            else
            {
                //it is a company user
                ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusDelayedPayment;
                ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusApproved;
            }
            int lastInsertedId = await _orderHeaderRepo.InsertOrderHeaderInfo(ShoppingCartVM.OrderHeader);


            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                OrderDetail orderDetail = new()
                {
                    ProductId = cart.Product.Id,
                    OrderHeaderId = lastInsertedId,
                    Price = cart.Price,
                    Count = cart.Count
                };
                await _orderDetailRepo.Save(orderDetail);
            }

            if (applicationUser.CompanyId.GetValueOrDefault() == 0)
            {
                //it is a regular customer account we need to capture payment
                //stripe logic
                var domain = "https://localhost:7290/";

                var options = new SessionCreateOptions
                {
                    SuccessUrl = domain + $"customer/cart/OrderConfirmation?id={lastInsertedId}",
                    CancelUrl = domain + "customer/cart/Index",
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment",
                };
                foreach (var item in ShoppingCartVM.ShoppingCartList)
                {
                    var sessionLineItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.Price * 100), // $20.50 => 2050
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Product.ProductName
                            }
                        },
                        Quantity = item.Count
                    };
                    options.LineItems.Add(sessionLineItem);
                }

                var service = new SessionService();
                Session session = service.Create(options);

                await _orderHeaderRepo.UpdateStripePaymentId(lastInsertedId, session.Id, session.PaymentIntentId);

                Response.Headers.Add("Location", session.Url);

                return new StatusCodeResult(303);
            }
            return RedirectToAction(nameof(OrderConfirmation), new { id = lastInsertedId });
        }

        public async Task<IActionResult> OrderConfirmation(int id)
        {
            OrderHeader orderHeader = await _orderHeaderRepo.GetOrderHeader(id);

            if (orderHeader.PaymentStatus != SD.PaymentStatusDelayedPayment)
            {
                //this is an order by customer

                var service = new SessionService();
                Session session = service.Get(orderHeader.SessionId);

                if (session.PaymentStatus.ToLower() == "paid")
                {
                    await _orderHeaderRepo.UpdateStripePaymentId(id, session.Id, session.PaymentIntentId);
                    await _orderHeaderRepo.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusApproved);
                }
                HttpContext.Session.Clear();
            }
            
            var result = await _cartRepo.DeleteCartItemByAppUserId(orderHeader.ApplicationUserId);
            
            return View(id);
        }

        public async Task<IActionResult> Plus(int cartId)
        {
            var cartFromDb = await _cartRepo.GetCart(cartId);
            cartFromDb.Count += 1;

            await _cartRepo.Edit(cartFromDb);

            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Minus(int cartId)
        {
            var cartFromDb = await _cartRepo.GetCart(cartId);
           
            if (cartFromDb.Count <= 1)
            {
                var getCartList = await _cartRepo.GetCartList(cartFromDb.ApplicationUserId);

                HttpContext.Session.SetInt32(SD.SessionCart, getCartList.Count() - 1);

                await _cartRepo.Delete(cartFromDb.Id);
            }
            else
            {
                cartFromDb.Count -= 1;
            }
            await _cartRepo.Edit(cartFromDb);

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Remove(int cartId)
        {
            var cartFromDb = await _cartRepo.GetCart(cartId);

            var getCartList = await _cartRepo.GetCartList(cartFromDb.ApplicationUserId);

            HttpContext.Session.SetInt32(SD.SessionCart, getCartList.Count() - 1);

            await _cartRepo.Delete(cartId);

            return RedirectToAction(nameof(Index));

        }
        private double GetPriceBasedOnQuantity(ShoppingCart shoppingCart)
        {
            if (shoppingCart.Count <= 50)
            {
                return shoppingCart.Product.Price;
            }
            else
            {
                if (shoppingCart.Count <= 100)
                {
                    return shoppingCart.Product.Price50;
                }
                else
                {
                    return shoppingCart.Product.Price100;
                }
            }

        }
    }
}
