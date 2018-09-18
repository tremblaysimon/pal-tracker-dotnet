using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq;

namespace PalTracker
{
    public class MySqlTimeEntryRepository : ITimeEntryRepository 
    {
        private readonly TimeEntryContext _context;
        public MySqlTimeEntryRepository(TimeEntryContext context)
        {
            _context = context;
        }

        public bool Contains(long id)
        {
            bool found = _context.TimeEntryRecords.AsNoTracking().Any(t => t.Id == id);

            return found;
        }

        public TimeEntry Create(TimeEntry timeEntry)
        {
            TimeEntryRecord record = timeEntry.ToRecord();
            
            EntityEntry<TimeEntryRecord> entity = _context.Add<TimeEntryRecord>(record);
            
            _context.SaveChanges();

            return entity.Entity.ToEntity();
        }

        public void Delete(long id)
        {
            TimeEntryRecord timeEntryRecord = _context.TimeEntryRecords.AsNoTracking().Single(t => t.Id == id);

            if (timeEntryRecord != null)
            {
                _context.Remove(timeEntryRecord);
                _context.SaveChanges();
            }

        }

        public TimeEntry Find(long id)
        {
            TimeEntryRecord timeEntryRecord = _context.TimeEntryRecords.AsNoTracking().Single(t => t.Id == id);

            return timeEntryRecord.ToEntity();
        }

        public IEnumerable<TimeEntry> List()
        {
            IEnumerable<TimeEntry> timeEntryRecordsToReturn = _context.TimeEntryRecords.AsNoTracking().Select(t => t.ToEntity());

            return timeEntryRecordsToReturn;
        }

        public TimeEntry Update(long id, TimeEntry timeEntry)
        {
            TimeEntryRecord timeEntryRecord = _context.Find<TimeEntryRecord>(id);
            timeEntryRecord.UserId = timeEntry.UserId;
            timeEntryRecord.ProjectId = timeEntry.ProjectId;
            timeEntryRecord.Date = timeEntry.Date;
            timeEntryRecord.Hours = timeEntry.Hours;

            _context.Update(timeEntryRecord);
            _context.SaveChanges();

            return timeEntryRecord.ToEntity();
        }
    }
}