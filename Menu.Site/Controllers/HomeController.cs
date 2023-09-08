using Menu.Site.Models;
using Menu.Site.Repository;
using Menu.Site.ViewModels;
using Menu.Site.ViewModels.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace Menu.Site.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAdminUserRepository _adminUserRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;

        public HomeController(IAdminUserRepository adminUserRepository, ICategoryRepository categoryRepository, IProductRepository productRepository)
        {
            _adminUserRepository = adminUserRepository;
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryRepository.GetAllSortedWithProductsAsync();
            return View(categories);
        }

        [Route("~/Admin")]
        [HttpGet]
        public IActionResult Admin()
        {
            return View();
        }
        [Route("~/Admin")]
        [HttpPost]
        public async Task<IActionResult> Admin(LoginVM pageLogin)
        {
            if (ModelState.IsValid)
            {
                pageLogin.Username.Trim();
                pageLogin.Pass.Trim();
                pageLogin.Pass= _adminUserRepository.GetHashCode(_adminUserRepository.GetHashCode(pageLogin.Pass));
                
                AdminUser? user = await _adminUserRepository.GetByUsernameAsync(pageLogin.Username);
                if (user != null && user.PassHash == pageLogin.Pass)
                {
                    ViewBag.IsError = false;
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                        new Claim(ClaimTypes.Name,user.UserName)
                    };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    var properties = new AuthenticationProperties()
                    {
                        IsPersistent = true
                    };
                    await HttpContext.SignInAsync(principal, properties);

                    return Redirect("Admin/Dashboard");
                }
                ViewBag.IsError = true;
            }
            return View(pageLogin);
        }

        [Route("~/Logout")]
        public IActionResult Logout()
        {
            AuthenticationHttpContextExtensions.SignOutAsync(HttpContext
                   , CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }


    }
}