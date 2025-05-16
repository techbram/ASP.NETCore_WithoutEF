using App.DataAccess.Repository.Contract;
using App.Models;
using ASP.NETCore_WithoutEF.App.DataAccess.Repository.Contract;
using ASP.NETCore_WithoutEF.App.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NETCore_WithoutEF.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IApplicationUserRepository<ApplicationUser> _userRepo;
        private readonly ICompanyRepository<Company> _companyRepo;
        private readonly IRoleRepository<Role> _roleRepo;
        public UserController(IApplicationUserRepository<ApplicationUser> userRepo, IRoleRepository<Role> roleRepo, ICompanyRepository<Company> compRepo)
        {
            _userRepo = userRepo;
            _companyRepo = compRepo;
            _roleRepo = roleRepo;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            List<ApplicationUser> applicationUsers = await _userRepo.GetUserList();
         
            return StatusCode(StatusCodes.Status200OK, applicationUsers);
        }
        //[HttpGet]
        //public async Task<IActionResult> GetRoles()
        //{
        //    List<Role> role = await _roleRepo.GetRoleList();

        //    return StatusCode(StatusCodes.Status200OK, role);
        //}
        //[HttpGet]
        //public async Task<IActionResult> GetCompany()
        //{
        //    List<Company> company = await _companyRepo.GetCompanyList();

        //    return StatusCode(StatusCodes.Status200OK, company);
        //}

    }
}
