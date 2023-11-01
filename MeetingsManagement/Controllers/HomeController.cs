using MeetingsManagementWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MeetingsManagementWeb.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        public IActionResult Index()
        {
            if (User.Identity is not null && User.Identity.IsAuthenticated)
                return RedirectToAction("Show", "Meetings");
            return View("AnonymousHome");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}