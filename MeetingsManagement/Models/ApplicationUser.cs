using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MeetingsManagementWeb.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public string? Nickname { get; set; }
    }
}
