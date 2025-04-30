using ASP.NETCore_WithoutEF.App.Models;
using ASP.NETCore_WithoutEF.App.DataAccess.Repository.Contract;
using ASP.NETCore_WithoutEF.App.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Reflection;


namespace ASP.NETCore_WithoutEF.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {

        private readonly IProductRepository<Product> _productRepo;
        private readonly IShoppingCartRepository<ShoppingCart> _cartRepo;
        private readonly IApplicationUserRepository<ApplicationUser> _userRepo;
        string userId = "";


        //string userId = "user123";
        //string userId = "admin123";
        //string userId = "testcompany123";
        public HomeController(IProductRepository<Product> productRepo, 
            IShoppingCartRepository<ShoppingCart> cartRepo, 
            IApplicationUserRepository<ApplicationUser> userRepo)
        {
            _productRepo = productRepo;
            _cartRepo = cartRepo;
            _userRepo = userRepo;
        }

        public async Task<IActionResult> Index()
        {
            //if (!string.IsNullOrEmpty(HttpContext.Session.GetString("userId")))
            //{
            //    userId = HttpContext.Session.GetString("userId").ToString();
            //}

            List<Product> productList = await _productRepo.GetProductList();
           
            return View(productList);
        }


        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            //if (!string.IsNullOrEmpty(HttpContext.Session.GetString("userId")))
            //{
            //    userId = HttpContext.Session.GetString("userId").ToString();
            //}

            ShoppingCart cart = new ShoppingCart()
            {
                Product = await _productRepo.GetProduct(id),
                Count = 1,
                ProductId = id
            };

            return View(cart);
        }


        [HttpPost]
        [ActionName("Detail")]
        //[Authorize]
        public async Task<IActionResult> DetailAddToCart(ShoppingCart shoppingCart)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new { success = false, message = "Please log in to add items to your cart." });
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { success = false, message = "User ID not found." });
            }

            shoppingCart.ApplicationUserId = userId;

            bool _result = await _cartRepo.Save(shoppingCart);
            var getCartList = await _cartRepo.GetCartList(userId);

            HttpContext.Session.SetInt32(SD.SessionCart, getCartList.Count());

            if (_result)
            {
                return Json(new { success = true, message = "Cart saved successfully!" });
            }

            return Json(new { success = false, message = "An error occurred while saving the cart!" });

            //if (!string.IsNullOrEmpty(HttpContext.Session.GetString("userId")))
            //{
            //    userId = HttpContext.Session.GetString("userId").ToString();
            //}
            //shoppingCart.ApplicationUserId = userId;

            //bool _result = await _cartRepo.Save(shoppingCart);

            //var getCartList = await _cartRepo.GetCartList(userId);

            //HttpContext.Session.SetInt32(SD.SessionCart, getCartList.Count());

            //if (_result)
            //{
            //    // Return a success response
            //    return Json(new { success = true, message = "Cart saved successfully!" });

            //}
            //// Return an error response
            //return Json(new { success = false, message = "An error occurred while saving the cart!" });

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
