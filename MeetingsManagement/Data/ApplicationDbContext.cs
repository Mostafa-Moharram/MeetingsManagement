using MeetingsManagementWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MeetingsManagementWeb.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Meeting>().HasMany<Reminder>().WithOne().HasForeignKey(reminder => reminder.MeetingId);
            builder.Entity<ReminderType>().HasMany<Reminder>().WithOne().HasForeignKey(r => r.TypeId);
            builder.Entity<ReminderType>().HasData(
                new ReminderType { Id = 1, Type = "Email" },
                new ReminderType { Id = 2, Type = "SMS" });
        }
        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<ReminderType> ReminderTypes { get; set; }
        public DbSet<Reminder> Reminders { get; set; }
    }
}
