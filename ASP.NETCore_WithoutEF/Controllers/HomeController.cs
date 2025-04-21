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

        private readonly IProductRepository<Product> _productRepo;
        private readonly IShoppingCartRepository<ShoppingCart> _cartRepo;
        private readonly IApplicationUserRepository<ApplicationUser> _userRepo;
        //string userId = "user123";
        //string userId = "admin123";
        string userId = "testcompany123";
        public HomeController(IProductRepository<Product> productRepo, IShoppingCartRepository<ShoppingCart> cartRepo, IApplicationUserRepository<ApplicationUser> userRepo)
        {
            _productRepo = productRepo;
            _cartRepo = cartRepo;
            _userRepo = userRepo;
        }

        public async Task<IActionResult> Index()
        {
            List<Product> productList = await _productRepo.GetProductList();
            //List<ShoppingCart> cartList = await _cartRepo.List();
            return View(productList);
        }
       

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
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
        public async Task<IActionResult> DetailAddToCart(ShoppingCart shoppingCart)
        {

           ApplicationUser applicationUser = await _userRepo.GetUser(userId);
         
            shoppingCart.ApplicationUserId = applicationUser.Id;

            bool _result = await _cartRepo.Save(shoppingCart);
            if (_result)
            {
                // Return a success response
                return Json(new { success = true, message = "Cart saved successfully!" });

            }
            // Return an error response
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
