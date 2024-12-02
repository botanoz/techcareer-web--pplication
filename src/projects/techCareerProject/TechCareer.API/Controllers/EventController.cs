using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using TechCareer.Service.Abstracts;
using TechCareer.Service.Concretes;
using Core.Security.Entities;

namespace TechCareer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool includeDeleted = false)
        {
            var events = await _eventService.GetListAsync(
                withDeleted: includeDeleted);
            return Ok(events);
        }

     
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var Event = await _eventService.GetAsync(x => x.Id == id);
            if (Event == null)
                return NotFound(new { Message = "event not found." });

            return Ok(Event);
        }

  
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Event Event)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var AddedEvent = await _eventService.AddAsync(Event);
            return CreatedAtAction(nameof(GetById), new { id = AddedEvent.Id }, AddedEvent);
        }

    
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Event Event)
        {
            if (id != Event.Id)
                return BadRequest(new { Message = "event ID mismatch." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var UpdatedEvent = await _eventService.UpdateAsync(Event);
                return Ok(UpdatedEvent);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "Event not found." });
            }
        }

   
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, [FromQuery] bool permanent = false)
        {
            try
            {
                var Event = new Event { Id = id };
                var deletedEvent = await _eventService.DeleteAsync(Event, permanent);
                return Ok(deletedEvent);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "event not found." });
            }
        }

   
        [HttpGet("paginate")]
        public async Task<IActionResult> GetPaginated(
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 10,
            [FromQuery] bool includeDeleted = false)
        {
            var result = await _eventService.GetPaginateAsync(
                index: pageIndex,
                size: pageSize,
                withDeleted: includeDeleted);

            return Ok(result);
        }
    }
}

