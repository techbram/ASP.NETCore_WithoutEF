using ASP.NETCore_WithoutEF.App.Models;
using ASP.NETCore_WithoutEF.App.DataAccess.Repository.Contract;
using Microsoft.AspNetCore.Mvc;
using ASP.NETCore_WithoutEF.App.Utility;
using Microsoft.AspNetCore.Authorization;

namespace ASP.NETCore_WithoutEF.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly ICompanyRepository<Company> _companyRepo;

        public CompanyController(ICompanyRepository<Company> companyRepo)
        {
            _companyRepo = companyRepo;
        }
        public IActionResult Index()
        {
            return View();
        }
     
        [HttpGet]
        public async Task<IActionResult> GetListCompanies()
        {
            List<Company> companyList = await _companyRepo.GetCompanyList();

            return StatusCode(StatusCodes.Status200OK, companyList);  
        }

        [HttpPost]
        public async Task<IActionResult> SaveCompany([FromBody] Company model)
        {

            if (ModelState.IsValid)
            {               

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
