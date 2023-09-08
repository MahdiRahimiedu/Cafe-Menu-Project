using Menu.Site.Repository;
using Menu.Site.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Menu.Site.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class UsersController : Controller
    {
        private readonly IAdminUserRepository _adminUserRepository;

        public UsersController(IAdminUserRepository adminUserRepository)
        {
            _adminUserRepository = adminUserRepository;
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            
            ChangePasswordVM vm  =new ChangePasswordVM();
            vm.Username = HttpContext.User.Identity.Name;
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM vm)
        {
            if(!ModelState.IsValid)
                return View(vm);

            var response = await _adminUserRepository.ChangePassword(vm);

            if(!response.Success)
                return View(vm);

            return Redirect("/Admin/Dashboard");
        }
    }
}
