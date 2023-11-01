using System.ComponentModel.DataAnnotations;

namespace MeetingsManagementWeb.Models.View
{
    public class UserPasswordDto
    {
        [Required, DataType(DataType.Password)]
        public string? CurrentPassword { get; set; }
        [Required, DataType(DataType.Password)]
        public string? NewPassword { get; set; }
        [Required, Compare("NewPassword")]
        public string? NewPasswordConfirmation { get; set; }
    }
    public class PhoneNumberDto
    {
        [Required, DataType(DataType.Password)]
        public string? Password {  get; set; }
        [Required]
        public string? PhoneNumber { get; set; }
    }
    public class UserProfileDto
    {
        public UserProfileDto(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));
            Name = name;
        }
        public string Name { get; init; }
        public UserPasswordDto UserPasswordDto { get; } = new UserPasswordDto();
        public PhoneNumberDto PhoneNumberDto { get; } = new PhoneNumberDto();
    }
}
