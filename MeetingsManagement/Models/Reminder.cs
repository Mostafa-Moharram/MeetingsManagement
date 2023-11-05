using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeetingsManagementWeb.Models
{
    public class Reminder
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
        [Required]
        public int TypeId { get; set; }
        public int MeetingId { get; set; }
    }
}
