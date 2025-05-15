using ASP.NETCore_WithoutEF.App.Models;
using ASP.NETCore_WithoutEF.App.DataAccess.Repository.Contract;
using Microsoft.AspNetCore.Mvc;
using ASP.NETCore_WithoutEF.App.DataAccess.Repository.Implementation;
using ASP.NETCore_WithoutEF.App.Utility;
using Microsoft.AspNetCore.Authorization;

namespace ASP.NETCore_WithoutEF.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {

        private readonly ICategoryRepository<Category> _categoryRepo;

        public CategoryController(ICategoryRepository<Category> categoryRepo)
        {

            _categoryRepo = categoryRepo;
        }
        public IActionResult Index()
        {
            return View();
        }
             
        [HttpGet]
        public async Task<IActionResult> GetListCategories()
        {
            List<Category> categoryList = await _categoryRepo.GetCategoryList();

            return StatusCode(StatusCodes.Status200OK, categoryList);    
        }
                
        [HttpPost]
        public async Task<IActionResult> SaveCategory([FromBody] Category model)
        {

            
            if (ModelState.IsValid)
            {
                bool _result = await _categoryRepo.SaveCategory(model);

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

       
        [HttpPut]
        public async Task<IActionResult> EditCategory([FromBody] Category model)
        {
            bool _result = await _categoryRepo.EditCategory(model);

            if (_result)
            {
                return StatusCode(StatusCodes.Status200OK, new { value = _result, msg = "ok" });
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { value = _result, msg = "error" });
            }

        }
      
        [HttpDelete]
        public async Task<IActionResult> DeleteCategory(int idCategory)
        {

            bool _result = await _categoryRepo.DeleteCategory(idCategory);

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
