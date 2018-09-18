using Microsoft.EntityFrameworkCore;

namespace PalTracker
{
    public class TimeEntryContext : DbContext
    {
        public TimeEntryContext(DbContextOptions option) : base(option)
        {}

        public DbSet<TimeEntryRecord> TimeEntryRecords {get; set;}
    }
}