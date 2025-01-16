using ASP.NETCore_WithoutEF.Models;
using ASP.NETCore_WithoutEF.Repositories.Contract;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NETCore_WithoutEF.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IGenericRepository<Category> _categoryRepo;

        public CategoryController(IGenericRepository<Product> productRepo, IGenericRepository<Category> categoryRepo)
        {
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
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

        // Method HttpPost to save the Category
        [HttpPost]
        public async Task<IActionResult> SaveCategory([FromBody] Category model)
        {
            bool _result = await _categoryRepo.Save(model);

            if (_result)
            {
                return StatusCode(StatusCodes.Status200OK, new { value = _result, msg = "ok" });

            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { value = _result, msg = "error" });
            }

        }

        //Method to Edit Category
        [HttpPut]
        public async Task<IActionResult> EditCategory([FromBody] Category model)
        {
            bool _result = await _categoryRepo.Edit(model);

            if (_result)
            {
                return StatusCode(StatusCodes.Status200OK, new { value = _result, msg = "ok" });
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { value = _result, msg = "error" });
            }

        }

        // Method httpdelete to delete Category
        [HttpDelete]
        public async Task<IActionResult> DeleteCategory(int idCategory)
        {

            bool _result = await _categoryRepo.Delete(idCategory);

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
