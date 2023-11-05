using MeetingsManagementWeb.Data;
using MeetingsManagementWeb.Models;
using MeetingsManagementWeb.Models.View;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace MeetingsManagementWeb.Controllers
{
    public class RemindersController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        public RemindersController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }
        [Authorize, Route("Meeting/{id}/Reminders", Name = "MeetingReminders")]
        public IActionResult Show(int id)
        {
            var meeting = _dbContext.Meetings.Find(id);
            if (meeting is null)
                return BadRequest(new { message = $"No meeting with Id = {id} exists." });
            var reminders = new RemindersVM {
                MeetingId = meeting.Id,
                MeetingTitle = meeting.Title,
                MeetingDescription = meeting.Description,
                StartTime = meeting.StartTime!.Value,
                EndTime = meeting.EndTime!.Value,
                Reminders = _dbContext.Reminders
                    .Where(reminder => reminder.MeetingId == id)
                    .OrderBy(reminder => reminder.DateTime)
                    .Select(reminder => new ReminderVM {
                        Date = reminder.DateTime.ToString("yyyy-MM-dd"),
                        Time = reminder.DateTime.ToString("hh:mm tt"),
                        TypeId = reminder.TypeId,
                        Id = reminder.Id
                    }),
                RemindersTypes = _dbContext.ReminderTypes
            };
            return View("MeetingReminders", reminders);
        }
        [Authorize, HttpPost]
        public IActionResult Create([FromBody] Reminder newReminder)
        {
            var meeting = _dbContext.Meetings.Find(newReminder.MeetingId);
            if (meeting is null)
                return BadRequest(new { status = "Failed", message = $"No meeting with Id = `{newReminder.MeetingId}` is found." });
            if (meeting.StartTime < newReminder.DateTime)
                return BadRequest(new {
                    status = "Failed",
                    message = "Cannot create a reminder with earlier time than the meeting start." });
            var reminderType = _dbContext.ReminderTypes.Find(newReminder.TypeId);
            if (reminderType is null)
                return BadRequest(new {
                    status = "Failed",
                    message = "Cannot create a reminder with unidentified type."
                });
            _dbContext.Reminders.Add(newReminder);
            _dbContext.SaveChanges();
            return Created("", JsonSerializer.Serialize(new ReminderVM {
                Date = newReminder.DateTime.ToString("yyyy-MM-dd"),
                Time = newReminder.DateTime.ToString("hh:mm tt"),
                TypeId = newReminder.TypeId,
                Id = newReminder.Id
            }));
        }
        [Authorize, HttpDelete]
        public IActionResult Delete(int id)
        {
            var reminder = _dbContext.Reminders.Find(id);
            if (reminder is null)
                return NotFound();
            var meeting = _dbContext.Meetings.Find(reminder.MeetingId);
            if (_userManager.GetUserId(HttpContext.User) != meeting!.UserId)
                return Unauthorized();
            _dbContext.Reminders.Remove(reminder);
            _dbContext.SaveChanges();
            return NoContent();
        }
    }
}
