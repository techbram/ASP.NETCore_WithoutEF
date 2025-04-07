using ASP.NETCore_WithoutEF.Models;
using ASP.NETCore_WithoutEF.Repository.Contract;
using Microsoft.AspNetCore.Mvc;
using Stripe.Climate;

namespace ASP.NETCore_WithoutEF.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderHeaderRepository<OrderHeader> _orderHeaderRepo;
        private readonly IOrderDetailRepository<OrderDetail> _orderDetailRepo;

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
        public async Task<IActionResult> GetListOrderHeaders()
        {
            List<OrderHeader> listOrderHeader = await _orderHeaderRepo.GetListOrderHeader();

            return StatusCode(StatusCodes.Status200OK, listOrderHeader);  // Return successful answer          
        }
        public async Task<IActionResult> GetListOrderDetail()
        {
            List<OrderDetail> listOrderDetail = await _orderDetailRepo.GetListOrderDetail();

            return StatusCode(StatusCodes.Status200OK, listOrderDetail);  // Return successful answer          
        }
    }
}
