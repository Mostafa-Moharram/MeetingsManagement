using System.ComponentModel.DataAnnotations;

namespace MeetingsManagementWeb.Models.View
{
    public class UserLogInDto
    {
        [Required, EmailAddress]
        public string? Email { get; set; }
        [Required, DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
