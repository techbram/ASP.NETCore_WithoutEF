using ASP.NETCore_WithoutEF.Models;
using ASP.NETCore_WithoutEF.Repository.Contract;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NETCore_WithoutEF.Controllers
{
    public class ProductController : Controller
    {

        private readonly IGenericRepository<Product> _productRepo;
        private readonly IGenericRepository<Category> _categoryRepo;

        public ProductController(IGenericRepository<Product> productRepo, IGenericRepository<Category> categoryRepo)
        {
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
        }

        public IActionResult Index()
        {
            return View();
        }

        //Method httpget to get list of category
        [HttpGet]
        public async Task<IActionResult> GetListCategories()
        {
            List<Category> categoryList = await _categoryRepo.List();

            return StatusCode(StatusCodes.Status200OK, categoryList);  // Return successful answer          
        }

        // Method httpget to get list of Products
        [HttpGet]
        public async Task<IActionResult> GetListProducts()
        {
            List<Product> productList = await _productRepo.List();

            return StatusCode(StatusCodes.Status200OK, productList);  // Return successful answer          
        }

      
        // Method to Save Product
        [HttpPost]
        public async Task<IActionResult> SaveProduct([FromBody] Product model)
        {
            if (ModelState.IsValid)
            {
                bool _result = await _productRepo.Save(model);

                if (_result)
                {
                    return StatusCode(StatusCodes.Status200OK, new { value = _result, msg = "ok" });
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { value = _result, msg = "error" });
                }
            }
            else
            {
                return BadRequest();
            }
            
        }

        //Method to Edit Product
        [HttpPut]
        public async Task<IActionResult> EditProduct([FromBody] Product model)
        {
            bool _result = await _productRepo.Edit(model);

            if (_result)
            {
                return StatusCode(StatusCodes.Status200OK, new { value = _result, msg = "ok" });
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { value = _result, msg = "error" });
            }

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

        // Method httpdelete to delete Product
        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(int idProduct)
        {

            bool _result = await _productRepo.Delete(idProduct);

            if (_result)
            {
                return StatusCode(StatusCodes.Status200OK, new { value = _result, msg = "ok" });
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { value = _result, msg = "error" });
            }
        }
    }
}
