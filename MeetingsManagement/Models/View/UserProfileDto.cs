using System.ComponentModel.DataAnnotations;

namespace MeetingsManagementWeb.Models.View
{
    public class UserNicknameDto
    {
        [Required]
        public string? Nickname { get; set; }
        [Required]
        public string? Password { get; set; }
    }
    public class UserPasswordDto
    {
        [Required, DataType(DataType.Password)]
        public string? CurrentPassword { get; set; }
        [Required, DataType(DataType.Password)]
        public string? NewPassword { get; set; }
        [Required, Compare("NewPassword")]
        public string? NewPasswordConfirmation { get; set; }
    }
    public class UserPhoneNumberDto
    {
        [Required, DataType(DataType.Password)]
        public string? Password {  get; set; }
        [Required]
        public string? PhoneNumber { get; set; }
    }
}
