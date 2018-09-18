using Steeltoe.Common.HealthChecks;
using System.Linq;

namespace PalTracker
{
    public class TimeEntryHealthContributor : IHealthContributor
    {
        public string Id { get; } = "timeEntry";
        public const int MaxTimeEntries = 5;

        private readonly ITimeEntryRepository _repository;

        public TimeEntryHealthContributor(ITimeEntryRepository repo)
        {
            _repository = repo;
        }

        public HealthCheckResult Health()
        {
            var count = _repository.List().ToList().Count;
            var status = count < MaxTimeEntries ? HealthStatus.UP : HealthStatus.DOWN;

            var result = new HealthCheckResult();
            result.Status = status;
            result.Details.Add("count", count);
            result.Details.Add("threshold", MaxTimeEntries);
            result.Details.Add("status", status.ToString());

            return result;
        }
    }
}