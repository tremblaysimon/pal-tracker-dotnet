using System;
using Microsoft.AspNetCore.Mvc;

namespace PalTracker
{
    [Route("/time-entries")]
    public class TimeEntryController : ControllerBase
    {
        private readonly ITimeEntryRepository _repository;

public TimeEntryController(ITimeEntryRepository repository) {
    _repository = repository;
}
        [HttpGet("{id}", Name = "GetTimeEntry")]
        public IActionResult Read(int id)
        {
           if(! _repository.Contains(id))
           {
return new NotFoundResult();
           }
            return new OkObjectResult(_repository.Find(id));
            
        }
[HttpPost]
        public object Create([FromBody] TimeEntry toCreate)
        {
            TimeEntry timeEntry = _repository.Create(toCreate);
            return new CreatedAtRouteResult("GetTimeEntry", new {id = timeEntry.Id}, timeEntry);
        }
[HttpGet]
        public object List()
        {
            return new OkObjectResult(_repository.List());
        }
[HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] TimeEntry theUpdate)
        {
            if(! _repository.Contains(id))
            {
                return  NotFound();
            }
            return (IActionResult) Ok(_repository.Update(id, theUpdate));
        }
[HttpDelete("{id}")]
        public object Delete(int id)
        {
            if(! _repository.Contains(id))
            {
                return new NotFoundResult();
            }
            _repository.Delete(id);
            return new NoContentResult();
        }
    }
}