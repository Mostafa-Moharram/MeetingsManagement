using MeetingsManagementWeb.Data;
using MeetingsManagementWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MeetingsManagementWeb.Controllers
{
    public class MeetingsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        public MeetingsController(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }
        [Authorize]
        public IActionResult Schedule()
        {
            return View();
        }
        [Authorize]
        public IActionResult Reschedule(int id)
        {
            string? userId = _userManager.GetUserId(HttpContext.User);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();
            var meeting = _dbContext.Meetings.Find(id);
            if (meeting is null)
                return NotFound();
            if (meeting.UserId != userId)
                return Unauthorized();
            return View("Schedule", meeting);
        }
        [Authorize, HttpPost]
        public IActionResult Schedule(Meeting meeting)
        {
            meeting.UserId = _userManager.GetUserId(HttpContext.User)!;
            if (!ModelState.IsValid)
                return View();
            if (meeting.StartTime < DateTime.Now) {
                ModelState.AddModelError("StartTime", "The start time of meeting cannot be earlier the current time.");
                return View();
            }
            if (meeting.EndTime < meeting.StartTime)
            {
                ModelState.AddModelError("EndTime", "The end time cannot be earlier than the start time.");
                return View();
            }
            if (meeting.Id > 0)
                _dbContext.Update(meeting);
            else
                _dbContext.Add(meeting);
            _dbContext.SaveChanges();
            return RedirectToRoute("MeetingReminders", new { meeting.Id });
        }
        [Authorize]
        public IActionResult Show()
        {
            var userId = _userManager.GetUserId(HttpContext.User)!;
            return View(_dbContext.Meetings.Where(m => m.UserId == userId).OrderBy(meeting => meeting.StartTime).ToList());
        }
        [Authorize, HttpDelete]
        public IActionResult Delete([FromBody] int id)
        {
            var meeting = _dbContext.Meetings.FirstOrDefault(m => m.Id == id);
            if (meeting is null)
                return NotFound();
            _dbContext.Meetings.Remove(meeting);
            _dbContext.SaveChanges();
            return NoContent();
        }
    }
}
