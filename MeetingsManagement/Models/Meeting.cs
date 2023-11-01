using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MeetingsManagementWeb.Models
{
    public class Meeting
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Title { get; set; }
        public string? Description { get; set; }
        [Required]
        public DateTime? StartTime { get; set; }
        [Required]
        public DateTime? EndTime { get; set; }
        public string? UserId { get; set; }
        [ForeignKey("ApplicationUser"), NotMapped]
        public ApplicationUser? User { get; set; }
    }
}
