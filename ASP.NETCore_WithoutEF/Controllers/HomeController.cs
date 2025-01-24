using ASP.NETCore_WithoutEF.Models;

using ASP.NETCore_WithoutEF.Repository.Contract;
using ASP.NETCore_WithoutEF.Repository.Implementation;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ASP.NETCore_WithoutEF.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IGenericRepository<Category> _categoryRepo;

        public HomeController(ILogger<HomeController> logger, IGenericRepository<Product> productRepo, IGenericRepository<Category> categoryRepo)
        {
            _logger = logger;
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
        }

        public async Task< IActionResult> Index()
        {
            List<Product> productList = await _productRepo.List();
           
            return View(productList);
        }


        // Action to view the product details
        public async Task<IActionResult> Detail(int id)
        {
            Product product = await _productRepo.Detail(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
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
