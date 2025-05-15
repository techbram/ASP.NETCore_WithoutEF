using App.Models.ViewModel;
using ASP.NETCore_WithoutEF.App.DataAccess.Repository.Contract;
using ASP.NETCore_WithoutEF.App.Models;
using ASP.NETCore_WithoutEF.App.Utility;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Stripe.BillingPortal;
using App.DataAccess.Repository.Contract;
using App.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

namespace ASP.NETCore_WithoutEF.Areas.Identity.Controllers
{
    [Area("Identity")]
    
    public class AccountController : Controller
    {
        private readonly IApplicationUserRepository<ApplicationUser> _userRepo;
        private readonly IRoleRepository<Role> _roleRepo;
        private readonly ICompanyRepository<Company> _compyRepo;
        private readonly ILogger<AccountController> _logger;

        public AccountController(ILogger<AccountController> logger,
            IApplicationUserRepository<ApplicationUser> userRepo, IRoleRepository<Role> roleRepo, ICompanyRepository<Company> compyRepo)
        {
            _userRepo = userRepo;
            _roleRepo = roleRepo;
            _compyRepo = compyRepo;
            _logger = logger;
        }


        [HttpGet]
        public async Task<IActionResult> Register()
        {
            List<Role> roles = await _roleRepo.GetRoleList();
            List<Company> companies = await _compyRepo.GetCompanyList();

            RegisterVM registerVM = new RegisterVM()
            {
                User = new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString()
                },
                RoleList = roles.Select(r => new SelectListItem
                {
                    Value = r.Id,
                    Text = r.Name
                }).ToList()
                ,
                CompanyList = companies.Select(c=> new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList()

            };

            return View(registerVM);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (!ModelState.IsValid)
            {
                List<Role> roles = await _roleRepo.GetRoleList();
                List<Company> companies = await _compyRepo.GetCompanyList();

                model.RoleList = roles.Select(r => new SelectListItem
                {
                    Value = r.Id,
                    Text = r.Name
                }).ToList();

                model.CompanyList = companies.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();

                return View(model);
            }

            var userId = await _userRepo.SaveUserAndReturnId(model.User);

            //add notification that new user is created by the admin HERE.

            if (string.IsNullOrEmpty(userId))
            {
                ModelState.AddModelError("", "Error creating user.");
                return View(model);
            }

            return RedirectToAction("Index", "Home", new { area = "Customer" });

        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginVM loginVM = new LoginVM();
            return View(loginVM);
        }
       

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {

            if (ModelState.IsValid)
            {
                UserInfo obj = await _userRepo.ValidateUser(model.Email, model.Password);

                if (!string.IsNullOrEmpty(obj.UserId))
                {
                    HttpContext.Session.SetString("UserId", obj.UserId);

                    HttpContext.Session.SetString("RoleName", obj.RoleName);

                    HttpContext.Session.SetString("UserName", obj.UserName);

                    return RedirectToAction("Index", "Home", new { area = "Customer" });
                   
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult ExternalLogin(string provider)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };

            return Challenge(properties, provider);
        }

        //[HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(LoginVM model)
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!result.Succeeded || result.Principal == null)
                return RedirectToAction("Login", "Account", new { area = "Identity" });

            UserInfo obj = await _userRepo.ValidateUser(model.Email, model.Password);

            if (!string.IsNullOrEmpty(obj.UserId))
            {
                HttpContext.Session.SetString("UserId", obj.UserId);

                HttpContext.Session.SetString("RoleName", obj.RoleName);

                HttpContext.Session.SetString("UserName", obj.UserName);

                return RedirectToAction("Index", "Home", new { area = "Customer" });

            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            //var externalUserId = result.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //var email = result.Principal.FindFirst(ClaimTypes.Email)?.Value;
            //var name = result.Principal.FindFirst(ClaimTypes.Name)?.Value;

            //if (string.IsNullOrEmpty(email))
            //{
            //    // You may want to redirect to a registration form asking for email
            //    return RedirectToAction("Login");
            //}

            //// Check if user exists
            //var userInfo = await _userRepo.ValidateUser(model.Email, model.Password);
            //if (userInfo == null)
            //{
            //    // Optionally auto-register the user
            //    var newUser = new ApplicationUser
            //    {
            //        Id = Guid.NewGuid().ToString(),
            //        Email = email,
            //        Name = name,
            //        // Set other default values
            //    };
            //   await _userRepo.SaveUserAndReturnId(newUser);
            //}

            //// Create your session
            //HttpContext.Session.SetString("UserId", userInfo?.UserId ?? newUser.Id);
            //HttpContext.Session.SetString("UserName", userInfo?.UserName ?? newUser.Name);
            //HttpContext.Session.SetString("RoleName", userInfo?.RoleName ?? "Customer");

            return RedirectToAction("Index", "Home", new { area = "Customer" });
        }

        public IActionResult Logout()
        {
            string userId = HttpContext.Session.GetString("UserId");
            string roleName = HttpContext.Session.GetString("RoleName");
            string UserName = HttpContext.Session.GetString("UserName");
            string sessionId = HttpContext.Session.Id;
                     

            //await HttpContext.SignOutAsync("MyCookieAuth");
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home", new { area = "Customer" });
        }
    }
}
