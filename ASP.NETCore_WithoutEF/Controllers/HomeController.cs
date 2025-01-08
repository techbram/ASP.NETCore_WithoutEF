using ASP.NETCore_WithoutEF.Models;
using ASP.NETCore_WithoutEF.Repositories.Contract;
using ASP.NETCore_WithoutEF.Repositories.Implementation;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ASP.NETCore_WithoutEF.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;        
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IGenericRepository<Category> _categoryRepo;

        public HomeController(ILogger<HomeController> logger,            
            IGenericRepository<Category> categoryRepository, 
            IGenericRepository<Product> productRepository)
        {
            _logger = logger;
            _categoryRepo = categoryRepository;
            _productRepo = productRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        

        // Method httpget to get list of category
        [HttpGet]
        public async Task<IActionResult> GetListCategories()
        {
            List<Category> categoryList = await _categoryRepo.List();

            return StatusCode(StatusCodes.Status200OK, categoryList);  // Return successful answer          
        }

        // Method httpget to get list of employees
        [HttpGet]
        public async Task<IActionResult> GetListProducts()
        {
            List<Product> productList = await _productRepo.List();

            return StatusCode(StatusCodes.Status200OK, productList);  // Return successful answer          
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
