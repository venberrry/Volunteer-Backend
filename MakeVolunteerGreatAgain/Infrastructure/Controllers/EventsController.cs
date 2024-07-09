using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MakeVolunteerGreatAgain.Core.Entities;
using MakeVolunteerGreatAgain.Core.Services;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using MakeVolunteerGreatAgain.Core.Repositories.DTO;

namespace MakeVolunteerGreatAgain.Infrastructure.Controllers
{
    [ApiController]
    [Route("api")]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet("GetAllEvents")]
        public async Task<ActionResult<IEnumerable<Event>>> GetAllEvents()
        {
            var events = await _eventService.GetAllEventsAsync();
            
            var eventsToReturn = events.Select(e => new 
            {
                Id = e.Id,
                Title = e.Title,
                PhotoPath = e.PhotoPath,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                City = e.City,
                OrganizationId = e.OrganizationId
            }).ToList();

            return Ok(eventsToReturn);
        }

        [Authorize(Roles = "Organization")]
        [HttpPost("CreateEvent")]
        public async Task<IActionResult> CreateEvent([FromBody] EventCreateDTO eventModel)
        {
            var organizationsId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var createEvent = await _eventService.CreateEventAsync(eventModel, organizationsId);

            return Ok(createEvent);
        }

        
        [HttpGet("GetById/{id:int}")]
        public async Task<ActionResult<Event>> GetEventById(int id)
        {
            var eventItem = await _eventService.GetEventByIdAsync(id);
            if (eventItem == null)
            {
                return NotFound();
            }
            var eventToReturn = new
            {
                Id = eventItem.Id,
                OrganizationId = eventItem.OrganizationId,
                Title = eventItem.Title,
                PhotoPath = eventItem.PhotoPath,
                StartDate = eventItem.StartDate,
                EndDate = eventItem.EndDate,
                City = eventItem.City,
                Description = eventItem.Description
            };
            
            return Ok(eventToReturn);
        }
        
        [Authorize(Roles = "Organization")]
        [HttpPut("UpdateEvent/{id:int}")]
        public async Task<IActionResult> UpdateEvent([FromBody] UpdateEventDTO updatedEvent, int id)
        {
            var eventItem = await _eventService.UpdateEventAsync(updatedEvent, id);

            return Ok(eventItem);
        }
        
        //Можно добавить защиту от удаления под другими организациями
        [Authorize(Roles = "Organization")]
        [HttpDelete("Delete/{id:int}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var success = await _eventService.DeleteEventAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
