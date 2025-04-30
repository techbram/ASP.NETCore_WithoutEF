using ASP.NETCore_WithoutEF.App.Models;
using ASP.NETCore_WithoutEF.App.DataAccess.Repository.Contract;
using ASP.NETCore_WithoutEF.App.Utility;
using ASP.NETCore_WithoutEF.App.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;


namespace ASP.NETCore_WithoutEF.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IOrderHeaderRepository<OrderHeader> _orderHeaderRepo;
        private readonly IOrderDetailRepository<OrderDetail> _orderDetailRepo;
        //string userId = "user123";
        //string userId = "admin123";
        //string userId = "testcompany123";
        [BindProperty]
        public OrderVM OrderVM { get; set; }
        public OrderController(IOrderHeaderRepository<OrderHeader> orderHeaderRepo, IOrderDetailRepository<OrderDetail> orderDetailRepo)
        {
            _orderHeaderRepo = orderHeaderRepo;
            _orderDetailRepo = orderDetailRepo;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetListOrderHeaders(string status)
        {
            IEnumerable<OrderHeader> objOrderHeaders = await _orderHeaderRepo.GetListOrderHeader();


            switch (status)
            {
                case "pending":
                    objOrderHeaders = objOrderHeaders.Where(u => u.PaymentStatus == SD.PaymentStatusDelayedPayment);
                    break;
                case "inprocess":
                    objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == SD.StatusInProcess);
                    break;
                case "completed":
                    objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == SD.StatusShipped);
                    break;
                case "approved":
                    objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == SD.StatusApproved);
                    break;
                default:
                    break;

            }

            return StatusCode(StatusCodes.Status200OK, objOrderHeaders);  // Return successful answer          
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int orderId)
        {
            OrderVM = new()
            {
                OrderHeader = await _orderHeaderRepo.GetOrderHeader(orderId),
                OrderDetailList = await _orderDetailRepo.GetOrderDetail(orderId)

            };

            return View(OrderVM);
        }
        [HttpPost]
        //[Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public async Task<IActionResult> UpdateOrderDetail()
        {
            var orderHeaderFromDb = await _orderHeaderRepo.GetOrderHeader(OrderVM.OrderHeader.Id);

            orderHeaderFromDb.Name = OrderVM.OrderHeader.Name;
            orderHeaderFromDb.PhoneNumber = OrderVM.OrderHeader.PhoneNumber;
            orderHeaderFromDb.StreetAddress = OrderVM.OrderHeader.StreetAddress;
            orderHeaderFromDb.City = OrderVM.OrderHeader.City;
            orderHeaderFromDb.State = OrderVM.OrderHeader.State;
            orderHeaderFromDb.PostalCode = OrderVM.OrderHeader.PostalCode;
            if (!string.IsNullOrEmpty(OrderVM.OrderHeader.Carrier))
            {
                orderHeaderFromDb.Carrier = OrderVM.OrderHeader.Carrier;
            }
            if (!string.IsNullOrEmpty(OrderVM.OrderHeader.TrackingNumber))
            {
                orderHeaderFromDb.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;
            }
            await _orderHeaderRepo.Edit(orderHeaderFromDb);

            TempData["Success"] = "Order Details Updated Successfully.";

            return RedirectToAction(nameof(Detail), new { orderId = orderHeaderFromDb.Id });
        }
        [HttpPost]
        public async Task<IActionResult> StartProcessing()
        {
            await _orderHeaderRepo.UpdateStatus(OrderVM.OrderHeader.Id, SD.StatusInProcess);

            return RedirectToAction(nameof(Detail), new { orderId = OrderVM.OrderHeader.Id });
        }
        [HttpPost]
        public async Task<IActionResult> ShipOrder()
        {
            var orderHeader = await _orderHeaderRepo.GetOrderHeader(OrderVM.OrderHeader.Id);
            orderHeader.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;
            orderHeader.Carrier = OrderVM.OrderHeader.Carrier;
            orderHeader.OrderStatus = SD.StatusShipped;
            orderHeader.ShippingDate = DateTime.Now;
            if (orderHeader.PaymentStatus == SD.PaymentStatusDelayedPayment)
            {
                orderHeader.PaymentDueDate = DateTime.Now.AddDays(30);
            }
            await _orderHeaderRepo.Edit(orderHeader);

            return RedirectToAction(nameof(Detail), new { orderId = OrderVM.OrderHeader.Id });
        }
        [HttpPost]
        public async Task<IActionResult> CancleOrder()
        {
            var orderHeader = await _orderHeaderRepo.GetOrderHeader(OrderVM.OrderHeader.Id);

            if (orderHeader.PaymentStatus == SD.PaymentStatusApproved)
            {
                var options = new RefundCreateOptions
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = orderHeader.PaymentIntentId
                };

                var service = new RefundService();
                Refund refund = service.Create(options);

                await _orderHeaderRepo.UpdateStatus(orderHeader.Id, SD.StatusCancelled, SD.StatusRefunded);

            }
            else
            {
                await _orderHeaderRepo.UpdateStatus(orderHeader.Id, SD.StatusCancelled, SD.StatusCancelled);
            }

            TempData["Success"] = "Order Cancelled Successfully.";
            return RedirectToAction(nameof(Detail), new { orderId = OrderVM.OrderHeader.Id });

        }

        [ActionName("Detail")]
        [HttpPost]
        public async Task<IActionResult> Details_PAY_NOW()
        {
            OrderVM.OrderHeader = await _orderHeaderRepo.GetOrderHeader(OrderVM.OrderHeader.Id);

            OrderVM.OrderDetailList = await _orderDetailRepo.GetOrderDetail(OrderVM.OrderHeader.Id);
            var orderheaderId = OrderVM.OrderHeader.Id;
            //stripe logic
            var domain = "https://localhost:7290/";
            var options = new SessionCreateOptions
            {
                SuccessUrl = domain + $"order/PaymentConfirmation?orderHeaderId={orderheaderId}",
                CancelUrl = domain + $"order/detail?orderId={orderheaderId}",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
            };

            foreach (var item in OrderVM.OrderDetailList)
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
            await _orderHeaderRepo.UpdateStripePaymentId(OrderVM.OrderHeader.Id, session.Id, session.PaymentIntentId);

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }
        public async Task<IActionResult> PaymentConfirmation(int orderHeaderId)
        {
            OrderHeader orderHeader = await _orderHeaderRepo.GetOrderHeader(orderHeaderId);

            if (orderHeader.PaymentStatus == SD.PaymentStatusDelayedPayment)
            {
                //this is an order by company

                var service = new SessionService();
                Session session = service.Get(orderHeader.SessionId);

                if (session.PaymentStatus.ToLower() == "paid")
                {
                    await _orderHeaderRepo.UpdateStripePaymentId(orderHeaderId, session.Id, session.PaymentIntentId);
                    await _orderHeaderRepo.UpdateStatus(orderHeaderId, orderHeader.OrderStatus, SD.PaymentStatusApproved);
                }

            }
            return View(orderHeaderId);
        }
    }
}
