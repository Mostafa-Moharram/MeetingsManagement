using System.ComponentModel.DataAnnotations;

namespace MeetingsManagementWeb.Models.View
{
    public class UserSignUpDto
    {
        [Required]
        public string? Nickname { get; set; }
        [Required, EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? PhoneNumber { get; set; }
        [Required, DataType(DataType.Password)]
        public string? Password { get; set; }
        [Required, Compare("Password")]
        public string? PasswordConfirmation { get; set; }
    }
}
