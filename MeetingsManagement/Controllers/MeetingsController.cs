using MeetingsManagementWeb.Data;
using MeetingsManagementWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            var meeting = new Meeting { UserId = _userManager.GetUserId(HttpContext.User)! };
            return View(meeting);
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
            _dbContext.Add(meeting);
            _dbContext.SaveChanges();
            return Ok(new { status = "Succeeded" });
        }
        [Authorize]
        public IActionResult Show()
        {
            return View(_dbContext.Meetings.OrderBy(meeting => meeting.StartTime).ToList());
        }
        [Authorize, HttpDelete]
        public IActionResult Delete([FromBody] int id)
        {
            var meeting = _dbContext.Meetings.FirstOrDefault(m => m.Id == id);
            if (meeting is null)
                return Ok(new { status = "Failed", message = $"The meeting with Id = {id} doesn't exist." });
            _dbContext.Meetings.Remove(meeting);
            _dbContext.SaveChanges();
            return Ok(new { status = "Succeeded" });
        }
    }
}
