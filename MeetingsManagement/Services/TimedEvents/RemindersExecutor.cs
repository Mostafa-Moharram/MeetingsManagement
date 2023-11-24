using MeetingsManagementWeb.Data;
using MeetingsManagementWeb.Models;

namespace MeetingsManagementWeb.Services.TimedEvents
{
    public class RemindersExecutor : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly object _stateSwitchingLock;
        private bool _started;
        public RemindersExecutor(IServiceProvider serviceProvider)
        {
            _stateSwitchingLock = new object();
            _serviceProvider = serviceProvider;
            _started = false;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            lock (_stateSwitchingLock)
            {
                _started = true;
                _ = TimerCallback();
            }
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            lock (_stateSwitchingLock)
                _started = false;
            return Task.CompletedTask;
        }

        private async Task TimerCallback()
        {
            await Task.Delay(60000);
            lock (_stateSwitchingLock)
            {
                if (!_started)
                    return;
                ExecuteReminders().Wait();
            }
            _ = TimerCallback();
        }

        private async Task ExecuteReminders()
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var emailSender = scope.ServiceProvider.GetRequiredService<EmailSender>();
            using var transaction = await dbContext.Database.BeginTransactionAsync();
            var dueDateTime = DateTime.Now;
            var meetings = GetMeetingsOrderedById(dueDateTime, dbContext);
            var reminders = GetRemindersOrderedById(dueDateTime, dbContext);
            var reminderTypes = GetReminderTypes(dbContext);
            int j = 0;
            var reminderTasks = new List<Task>();
            foreach (var reminder in reminders)
            {
                while (meetings[j].Meeting.Id < reminder.MeetingId)
                    ++j;
                if (reminder.MeetingId < meetings[j].Meeting.Id)
                    throw new InvalidDataException();
                reminderTasks.Add(ExecuteReminder(reminder, reminderTypes[reminder.TypeId], meetings[j], emailSender));
            }
            await Task.WhenAll(reminderTasks);
            dbContext.RemoveRange(reminders);
            dbContext.SaveChanges();
            transaction.Commit();
        }

        public static Task ExecuteReminder(Reminder reminder, string reminderType, MeetingUserDto meetingUserDto, EmailSender emailSender)
        {
            string messageBody = $"Hello {meetingUserDto.UserNickname},\n\n" +
                $"This a gentle reminder set to be at {reminder.DateTime: yyyy-MM-dd hh:mm tt}" +
                $"for a meeting titled `{meetingUserDto.Meeting.Title}` " +
                $"having description `{meetingUserDto.Meeting.Description}` " +
                $"scheduled from `{meetingUserDto.Meeting.StartTime: yyyy-MM-dd hh:mm tt}` " +
                $"to `{meetingUserDto.Meeting.EndTime: yyyy-MM-dd hh:mm tt}`.";
            emailSender.Send(messageBody, meetingUserDto.UserEmail);
            return Task.CompletedTask;
        }

        private static MeetingUserDto[] GetMeetingsOrderedById(DateTime dateTime, ApplicationDbContext dbContext)
        {
            return dbContext.Meetings
                .Join(dbContext.Reminders
                    .Where(r => r.DateTime <= dateTime)
                    .Select(r => r.MeetingId)
                    .Distinct(),
                    m => m.Id,
                    r => r,
                    (m, _) => m)
                .Join(
                    dbContext.Users, m => m.UserId,
                    u => u.Id,
                    (m, u) => new MeetingUserDto
                    {
                        Meeting = m,
                        UserNickname = u.Nickname!,
                        UserEmail = u.Email!.Trim().ToLower(),
                        UserPhoneNumber = u.PhoneNumber!.Trim()
                    })
                .OrderBy(m => m.Meeting.Id)
                .ToArray();
        }

        private static Reminder[] GetRemindersOrderedById(DateTime dateTime, ApplicationDbContext dbContext)
        {
            return dbContext.Reminders
                .Where(r => r.DateTime <= dateTime)
                .OrderBy(r => r.MeetingId)
                .ToArray();
        }

        private static IDictionary<int, string> GetReminderTypes(ApplicationDbContext dbContext)
        {
            return new Dictionary<int, string>(dbContext.ReminderTypes.Select(rt => new KeyValuePair<int, string>(rt.Id, rt.Type!)));
        }
    }
}
