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
        public async Task<ActionResult<IEnumerable<Event>>> GetAllEvents([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
             if (page <= 0 || pageSize <= 0)
            {
                return BadRequest("Page and pageSize must be positive integers.");
            }

            var totalEvents = await _eventService.GetAllEventsAsync();
            var paginatedEvents = totalEvents
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new 
                {
                    Id = e.Id,
                    Title = e.Title,
                    PhotoPath = e.PhotoPath,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    City = e.City,
                    OrganizationId = e.OrganizationId
                })
                .ToList();

            var totalPages = (int)Math.Ceiling(totalEvents.Count() / (double)pageSize);

            Response.Headers.Append("X-Total-Count", totalEvents.Count().ToString()); // добавляем заголовок X-Total-Count в ответ 
            Response.Headers.Append("X-Total-Pages", totalPages.ToString()); // для пагинации

            return Ok(paginatedEvents);
        }

        [Authorize(Roles = "Organization")]
        [HttpPost("CreateEvent")]
        public async Task<IActionResult> CreateEvent([FromBody] EventCreateDTO eventModel)
        {
            var organizationsId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var createEvent = await _eventService.CreateEventAsync(eventModel, organizationsId);

            return Ok(createEvent);
        }

        [Authorize(Roles = "Organization")]
        [HttpGet("MyEvents")]
        public async Task<IActionResult> GetEventsForOrganization()
        {
            var organizationCommonUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Получаем мероприятия для организации по его CommonUserId
            var events = await _eventService.GetEventsForOrganizationAsync(organizationCommonUserId);

            var eventsToReturn = events.Select(s => new
            {
                s.Id,
                s.Title,
                s.City,
                s.PhotoPath,
                s.StartDate,
                s.EndDate
            }).ToList();
            return Ok(eventsToReturn);
        }

        
        [HttpGet("GetById/{id:int}")]
        public async Task<ActionResult<Event>> GetEventById(int id)
        {
            var eventItem = await _eventService.GetEventByIdAsync(id);
            if (eventItem == null)
            {
                return NotFound( new { Message = "Мероприятие не найдено" });
            }
            var eventToReturn = new
            {
                Id = eventItem.Id,
                OrganizationId = eventItem.OrganizationId,
                OrganizationName = eventItem.Organization.Name,
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

            return Ok( new { Message = "Мероприятие успешно обновлено" });
        }
        
        //Можно добавить защиту от удаления под другими организациями
        [Authorize(Roles = "Organization")]
        [HttpDelete("Delete/{id:int}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var success = await _eventService.DeleteEventAsync(id);
            if (!success)
            {
                return NotFound( new { Message = "Мероприятие не найдено" });
            }
            return Ok( new { Message = "Мероприятие успешно удалено" });
        }
    }
}
