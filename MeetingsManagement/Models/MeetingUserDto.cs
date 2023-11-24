namespace MeetingsManagementWeb.Models
{
    public class MeetingUserDto
    {
        public required Meeting Meeting { get; set; }
        public required string UserNickname { get; set; }
        public required string UserEmail { get; set; }
        public required string UserPhoneNumber { get; set; }
    }
}
