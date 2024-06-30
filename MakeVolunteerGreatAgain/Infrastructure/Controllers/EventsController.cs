using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MakeVolunteerGreatAgain.Core.Entities;
using MakeVolunteerGreatAgain.Core.Services;
using System.Collections.Generic;
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
            return Ok(events);
        }

        [HttpGet("GetById/{id:int}")]
        public async Task<ActionResult<Event>> GetEventById(int id)
        {
            var eventItem = await _eventService.GetEventByIdAsync(id);
            if (eventItem == null)
            {
                return NotFound();
            }
            return Ok(eventItem);
        }

        [Authorize(Roles = "Organization")]
        [HttpPost("CreateEvent")]
        public async Task<IActionResult> CreateEvent([FromBody] EventCreateDTO eventModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Получение идентификатора текущего пользователя
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Создание объекта мероприятия с использованием идентификатора пользователя
            var eventObj = new Event
            {
                Title = eventModel.Title,
                StartDate = eventModel.StartDate,
                EndDate = eventModel.EndDate,
                City = eventModel.City,
                Description = eventModel.Description,
                OrganizationId = userId // Установка OrganizationId как идентификатор текущего пользователя
            };

            var createdEvent = await _eventService.CreateEventAsync(eventObj);

            return CreatedAtAction(nameof(GetEventById), new { id = createdEvent.Id }, createdEvent);
        }

        [Authorize(Roles = "Organization")]
        [HttpPut("UpdateEvent/{id:int}")]
        public async Task<IActionResult> UpdateEvent(int id, Event updatedEvent)
        {
            var eventItem = await _eventService.UpdateEventAsync(id, updatedEvent);
            if (eventItem == null)
            {
                return NotFound();
            }
            return NoContent();
        }

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
