using System;
using Microsoft.AspNetCore.Mvc;

namespace PalTracker
{
    [Route("/time-entries")]
    public class TimeEntryController : ControllerBase
    {
        private readonly ITimeEntryRepository _repository;
        private IOperationCounter<TimeEntry> _operationCounter;

        public TimeEntryController(ITimeEntryRepository repository, IOperationCounter<TimeEntry> operationCounter) {
            _repository = repository;
            _operationCounter = operationCounter;
        }

        [HttpGet("{id}", Name = "GetTimeEntry")]
        public IActionResult Read(int id)
        {
            _operationCounter.Increment(TrackedOperation.Read);
           if(! _repository.Contains(id))
           {
                return new NotFoundResult();
           }
            return new OkObjectResult(_repository.Find(id));
            
        }
[HttpPost]
        public object Create([FromBody] TimeEntry toCreate)
        {
            _operationCounter.Increment(TrackedOperation.Create);
            TimeEntry timeEntry = _repository.Create(toCreate);
            return new CreatedAtRouteResult("GetTimeEntry", new {id = timeEntry.Id}, timeEntry);
        }
[HttpGet]
        public object List()
        {
            _operationCounter.Increment(TrackedOperation.List);
            return new OkObjectResult(_repository.List());
        }
[HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] TimeEntry theUpdate)
        {
            _operationCounter.Increment(TrackedOperation.Update);
            if(! _repository.Contains(id))
            {
                return  NotFound();
            }
            return (IActionResult) Ok(_repository.Update(id, theUpdate));
        }
[HttpDelete("{id}")]
        public object Delete(int id)
        {
            _operationCounter.Increment(TrackedOperation.Delete);
            if(! _repository.Contains(id))
            {
                return new NotFoundResult();
            }
            _repository.Delete(id);
            return new NoContentResult();
        }
    }
}