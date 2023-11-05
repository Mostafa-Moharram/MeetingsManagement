namespace MeetingsManagementWeb.Models.View
{
    public class RemindersVM
    {
        public int MeetingId { get; set; }
        public required string? MeetingTitle { get; set; }
        public string? MeetingDescription { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public IEnumerable<ReminderVM> Reminders { get; set; } = Enumerable.Empty<ReminderVM>();
        public IEnumerable<ReminderType> RemindersTypes { get; set; } = Enumerable.Empty<ReminderType>();
    }
    public class ReminderVM {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public required string Date {  get; set; }
        public required string Time { get; set; }
    }
}
