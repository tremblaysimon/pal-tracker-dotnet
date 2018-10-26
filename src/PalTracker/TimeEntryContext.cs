using System;
using Microsoft.EntityFrameworkCore;

namespace PalTracker
{
    public class TimeEntryContext : DbContext
    {
        public TimeEntryContext(DbContextOptions option) : base(option)
        {
            Console.WriteLine("Connection string is: " + Database.GetDbConnection().ConnectionString);
        }

        public DbSet<TimeEntryRecord> TimeEntryRecords {get; set;}
    }
}