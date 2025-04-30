using App.Models.ViewModel;
using ASP.NETCore_WithoutEF.App.DataAccess.Repository.Contract;
using ASP.NETCore_WithoutEF.App.Models;
using ASP.NETCore_WithoutEF.App.Utility;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Stripe.BillingPortal;

namespace ASP.NETCore_WithoutEF.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly IApplicationUserRepository<ApplicationUser> _userRepo;
        //string userId = "testcompany123";
        public AccountController(
            IApplicationUserRepository<ApplicationUser> userRepo)
        {
            _userRepo = userRepo;
        }


        [HttpGet]
        public IActionResult Register()
        {
            RegisterVM registerVM = new RegisterVM();
            return View(registerVM);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            bool _result = await _userRepo.SaveUser(model.User);

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
                string Id = await _userRepo.ValidateUser(model.Email, model.Password);
                if (!string.IsNullOrEmpty(Id))
                {
                    //HttpContext.Session.SetString("userId", Id);
                    //return RedirectToAction("Index", "Home", new { area = "Customer" });
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, model.Email),    //ClaimTypes.Name: Used for User.Identity.Name
                            new Claim(ClaimTypes.NameIdentifier, Id)   //Stores the user ID (User.FindFirst(ClaimTypes.NameIdentifier).Value).
                        };

                    var identity = new ClaimsIdentity(claims, "MyCookieAuth");
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync("MyCookieAuth", principal);

                    return RedirectToAction("Index", "Home", new { area = "Customer" });
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return View(model);
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("MyCookieAuth");
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home", new { area = "Customer" });
        }
    }
}
