using ASP.NETCore_WithoutEF.Models;

using ASP.NETCore_WithoutEF.Repository.Contract;
using ASP.NETCore_WithoutEF.Repository.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Diagnostics;
using System.Security.Claims;

namespace ASP.NETCore_WithoutEF.Controllers
{
    public class HomeController : Controller
    {

        private readonly IGenericRepository<Product> _productRepo;
        private readonly IGenericRepository<ShoppingCart> _cartRepo;
        public HomeController(IGenericRepository<Product> productRepo, IGenericRepository<ShoppingCart> cartRepo)
        {
            _productRepo = productRepo;
            _cartRepo = cartRepo;
        }

        public async Task<IActionResult> Index()
        {
            List<Product> productList = await _productRepo.List();
            //List<ShoppingCart> cartList = await _cartRepo.List();
            return View(productList);
        }
        [HttpGet]
        public async Task<IActionResult> GetShoppingCart()
        {
            List<ShoppingCart> cartList = await _cartRepo.List();

            return StatusCode(StatusCodes.Status200OK, cartList);  // Return successful answer          
        }

        // Action to view the product details on Home Page
        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            ShoppingCart cart = new ShoppingCart()
            {
                Product = await _productRepo.Detail(id),
                Count = 1,
                ProductId = id
            };

            return View(cart);
        }

        [HttpPost]
        [ActionName("Detail")]
        public async Task<IActionResult> Detail(ShoppingCart model)
        {

            bool _result = await _cartRepo.Save(model);
            if (_result)
            {
                // Return a success response
                return Json(new { success = true, message = "Cart saved successfully!" });

            }
            // Return an error response
            return Json(new { success = false, message = "An error occurred while saving the cart!" });
           
        }
        public async Task<IActionResult> EditCart(ShoppingCart model)
        {

            bool _result = await _cartRepo.Edit(model);

            if (_result)
            {
                return StatusCode(StatusCodes.Status200OK, new { value = _result, msg = "ok" });

            }
            return StatusCode(StatusCodes.Status500InternalServerError, new { value = _result, msg = "error" });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
