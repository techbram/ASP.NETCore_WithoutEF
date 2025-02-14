using ASP.NETCore_WithoutEF.Models;
using ASP.NETCore_WithoutEF.Repository.Contract;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NETCore_WithoutEF.Controllers
{
    public class CompanyController : Controller
    {

        private readonly IGenericRepository<Company> _companyRepo;

        public CompanyController(IGenericRepository<Company> companyRepo)
        {

            _companyRepo = companyRepo;
        }
        public IActionResult Index()
        {
            return View();
        }

        // Method httpget to get list of category
        [HttpGet]
        public async Task<IActionResult> GetListCompanies()
        {
            List<Company> companyList = await _companyRepo.List();

            return StatusCode(StatusCodes.Status200OK, companyList);  // Return successful answer          
        }



        // Method HttpPost to save the Company
        [HttpPost]
        public async Task<IActionResult> SaveCompany([FromBody] Company model)
        {

            // Check if the model state is valid
            if (ModelState.IsValid)
            {
                // If the model is valid, save the category

                bool _result = await _companyRepo.Save(model);

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

        //Method to Edit Company
        [HttpPut]
        public async Task<IActionResult> EditCompany([FromBody] Company model)
        {
            bool _result = await _companyRepo.Edit(model);

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
        public async Task<IActionResult> DeleteCompany(int idCompany)
        {

            bool _result = await _companyRepo.Delete(idCompany);

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
