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
        //string userId = "";
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
            string userId = HttpContext.Session.GetString("UserId");

            if (userId != null)
            {
                HttpContext.Session.SetInt32(SD.SessionCart, (await _cartRepo.GetCartList(userId)).Count());
            }
            List<Product> productList = await _productRepo.GetProductList();

            return View(productList);
        }


        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            ShoppingCart shoppingCart = new ShoppingCart()
            {
                Product = await _productRepo.GetProduct(id),
                Count = 1,
                ProductId = id
            };

            return View(shoppingCart);
        }


        [HttpPost]
        [ActionName("Detail")]
        public async Task<IActionResult> DetailAddToCart(ShoppingCart shoppingCart)
        {
            string userId = HttpContext.Session.GetString("UserId");

            if (userId == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Please login to add item!",
                    redirectUrl = Url.Action("Login", "Account", new { area = "Identity" }) // or wherever you want
                });
                //return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            shoppingCart.ApplicationUserId = userId;            

            bool _result = await _cartRepo.Save(shoppingCart);

            HttpContext.Session.SetInt32(SD.SessionCart, (await _cartRepo.GetCartList(userId)).Count());

            if (_result)
            {
                return Json(new
                {
                    success = true,
                    message = "Item added to cart!",
                    //redirectUrl = Url.Action("Index", "Home", new { area = "Customer" }) // or wherever you want
                });
            }

            return Json(new { success = false, message = "An error occurred while saving the cart!" });

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
