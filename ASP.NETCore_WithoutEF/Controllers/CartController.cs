using ASP.NETCore_WithoutEF.Models;
using ASP.NETCore_WithoutEF.Repository.Contract;
using ASP.NETCore_WithoutEF.Repository.Implementation;
using ASP.NETCore_WithoutEF.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace ASP.NETCore_WithoutEF.Controllers
{
	public class CartController : Controller
	{
		private readonly IGenericRepository<Product> _productRepo;
		private readonly IGenericRepository<ShoppingCart> _cartRepo;
		private readonly IGenericRepository<ApplicationUser> _userRepo;
		private readonly IGenericRepository<OrderHeader> _orderHeaderRepo;
		private readonly IGenericRepository<OrderDetail> _orderDetailRepo;
		//[BindProperty]
		public ShoppingCartVM ShoppingCartVM { get; set; }
		public CartController(
			IGenericRepository<Product> productRepo, 
			IGenericRepository<ShoppingCart> cartRepo,
			IGenericRepository<ApplicationUser> userRepo,
			IGenericRepository<OrderHeader> orderHeaderRepo,
			IGenericRepository<OrderDetail> orderDetailRepo)

		{
			_productRepo = productRepo;
			_cartRepo = cartRepo;
			_userRepo = userRepo;
			_orderHeaderRepo = orderHeaderRepo;
			_orderDetailRepo = orderDetailRepo;
		}
		public async Task<IActionResult> Index()
		{
			ShoppingCartVM = new()
			{
				ShoppingCartList = await _cartRepo.List(),
				//ProductList = await _productRepo.List(),
				OrderHeader = new()

			};
			foreach (var cart in ShoppingCartVM.ShoppingCartList)
			{
				cart.Price = GetPriceBasedOnQuantity(cart);
				ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
			}
			return View(ShoppingCartVM);
		}

		public async Task<IActionResult> Summary()
		{
			var usersTask = _userRepo.List();
			var cartsTask = _cartRepo.List();
			//var productsTask = _productRepo.List();

			await Task.WhenAll(usersTask, cartsTask);

			List<ApplicationUser> applicationUsers = usersTask.Result;
			List<ShoppingCart> shoppingCarts = cartsTask.Result;
			//List<Product> products = productsTask.Result;

			if (!applicationUsers.Any()) // Ensure at least one user exists
			{
				return BadRequest("No users found.");
			}

			//var user = applicationUsers.First(); // Select a single user

			var shoppingCartVM = new ShoppingCartVM()
			{
				ShoppingCartList = shoppingCarts,
				OrderHeader = new()
			};

			shoppingCartVM.OrderHeader.ApplicationUser = applicationUsers.First();

			shoppingCartVM.OrderHeader.Name = shoppingCartVM.OrderHeader.ApplicationUser.Name;
			shoppingCartVM.OrderHeader.StreetAddress = shoppingCartVM.OrderHeader.ApplicationUser.StreetAddress;
			shoppingCartVM.OrderHeader.City = shoppingCartVM.OrderHeader.ApplicationUser.City;
			shoppingCartVM.OrderHeader.State = shoppingCartVM.OrderHeader.ApplicationUser.State;
			shoppingCartVM.OrderHeader.PostalCode = shoppingCartVM.OrderHeader.ApplicationUser.PostalCode;
			shoppingCartVM.OrderHeader.PhoneNumber = shoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;

			

			foreach (var cart in shoppingCartVM.ShoppingCartList)
			{
				cart.Price = GetPriceBasedOnQuantity(cart);
				shoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
			}

			return View(shoppingCartVM);
		}

		[HttpPost]
		[ActionName("Summary")]
		public async Task<IActionResult> SummaryPost(ShoppingCartVM shoppingCartVM)
		{
			var usersTask = _userRepo.List();
			//var cartsTask = _cartRepo.List();
			var orderHeaderTask = _orderHeaderRepo.List();
			

			await Task.WhenAll(usersTask, orderHeaderTask);

			List<ApplicationUser> applicationUsers = usersTask.Result;
			//List<ShoppingCart> shoppingCarts = cartsTask.Result;
			List<OrderHeader> orderHeaders = orderHeaderTask.Result;
			

			if (!applicationUsers.Any()) // Ensure at least one user exists
			{
				return BadRequest("No users found.");
			}

			var user = applicationUsers.First(); // Select a single user

			shoppingCartVM.ShoppingCartList = await _cartRepo.List();
			
			shoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
			shoppingCartVM.OrderHeader.ShippingDate = DateTime.Now;
			shoppingCartVM.OrderHeader.PaymentDate = DateTime.Now;
			shoppingCartVM.OrderHeader.PaymentDueDate = DateTime.Now;

			shoppingCartVM.OrderHeader.ApplicationUserId = user.Id;		


			foreach (var cart in shoppingCartVM.ShoppingCartList)
			{
				cart.Price = GetPriceBasedOnQuantity(cart);
				shoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
			}
			if (user.CompanyId.GetValueOrDefault() == 0)
			{
				//Regular customer account                 
				shoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
				shoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
			}
			else
			{
				//company account               
				shoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusDelayedPayment;
				shoppingCartVM.OrderHeader.OrderStatus = SD.StatusApproved;
			}
			//Below code Creating OrderHeader

			 await _orderHeaderRepo.Save(shoppingCartVM.OrderHeader);
			
			//Below code Creating OrderDetail
			foreach (var cart in shoppingCartVM.ShoppingCartList)
			{

				OrderDetail orderDetail = new OrderDetail()
				{
					ProductId = cart.Product.Id,
					OrderHeaderId = 28,
					Price = cart.Price,
					Count = cart.Count,
				};
				await _orderDetailRepo.Save(orderDetail);
				
			}
			return RedirectToAction(nameof(OrderConfirmation), new { id = shoppingCartVM.OrderHeader.Id });
			//return View(shoppingCartVM);
		}

		public IActionResult OrderConfirmation(int id)
		{
			return View(id);
		}

		public async Task<IActionResult> Plus(int id)
		{

			ShoppingCartVM cart = new ShoppingCartVM()
			{
				ShoppingCart = await _cartRepo.Detail(id),
				//ShoppingCartId = id
			};

			cart.ShoppingCart.Count += 1;
			await _cartRepo.Edit(cart.ShoppingCart);


			return RedirectToAction(nameof(Index));

		}
		public async Task<IActionResult> Minus(int id)
		{

			ShoppingCartVM cart = new ShoppingCartVM()
			{
				ShoppingCart = await _cartRepo.Detail(id),
				//ShoppingCartId = id
			};


			if (cart.ShoppingCart.Count <= 1)
			{
				//remove the cart
				await _cartRepo.Delete(id);
			}
			else
			{
				cart.ShoppingCart.Count -= 1;
			}
			await _cartRepo.Edit(cart.ShoppingCart);

			return RedirectToAction(nameof(Index));
		}
		public async Task<IActionResult> Remove(int id)
		{
			ShoppingCartVM cart = new ShoppingCartVM()
			{
				ShoppingCart = await _cartRepo.Detail(id),
				//ShoppingCartId = id
			};

			//remove the cart
			await _cartRepo.Delete(id);
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
