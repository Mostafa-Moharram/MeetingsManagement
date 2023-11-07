using MeetingsManagementWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MeetingsManagementWeb.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public ProfileController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [Authorize]
        public IActionResult Index()
        {
            var user = _userManager.GetUserAsync(HttpContext.User).Result;
            if (user is null)
                return RedirectToAction("Index", "Home");
            return View(user.Nickname! as object);
        }
    }
}
