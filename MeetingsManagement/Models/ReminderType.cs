using System.ComponentModel.DataAnnotations;

namespace MeetingsManagementWeb.Models
{
    public class ReminderType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Type { get; set; }
    }
}
