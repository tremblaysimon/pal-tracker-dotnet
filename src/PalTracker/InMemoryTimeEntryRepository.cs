using System.Collections.Generic;
using System.Linq;

namespace PalTracker
{
    public class InMemoryTimeEntryRepository : ITimeEntryRepository
    {
        private IDictionary<long,TimeEntry> _timeEntries = new Dictionary<long,TimeEntry>();

        public bool Contains(long id)
        {
          return  _timeEntries.ContainsKey(id);
        }

        public TimeEntry Create(TimeEntry timeEntry)
        {
            var id = _timeEntries.Count +1;
            timeEntry.Id = id;
            _timeEntries.Add(id,timeEntry);
            return timeEntry;
        }

        public void Delete(long id)
        {
            _timeEntries.Remove(id);
        }

        public TimeEntry Find(long id)
        {
            TimeEntry timeEntry;
            _timeEntries.TryGetValue(id, out timeEntry);
           
            return timeEntry;
        }

        public IEnumerable<TimeEntry> List()
        {
            return _timeEntries.Values.ToList();
        }

        public TimeEntry Update(long id, TimeEntry timeEntry)
        {
            timeEntry.Id = id;
           _timeEntries.Remove(id);
           _timeEntries.Add(id, timeEntry);
           return timeEntry;
        }
    }
}