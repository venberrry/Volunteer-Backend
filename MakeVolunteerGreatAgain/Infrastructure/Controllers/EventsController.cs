using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MakeVolunteerGreatAgain.Core.Entities;
using MakeVolunteerGreatAgain.Core.Services;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using MakeVolunteerGreatAgain.Core.Repositories.DTO;
using MakeVolunteerGreatAgain.Infrastructure.Services.Redis;

namespace MakeVolunteerGreatAgain.Infrastructure.Controllers
{
    [ApiController]
    [Route("api")]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly ICacheService _cacheService;

        private const string AllEventsCacheKey = "AllEvents";
        private const string OrganizationEventsCacheKeyPrefix = "OrganizationEvents_";



        public EventsController(IEventService eventService, ICacheService cacheService)
        {
            _eventService = eventService;
            _cacheService = cacheService;
        }

        [HttpGet("GetAllEvents")]
        public async Task<ActionResult<IEnumerable<Event>>> GetAllEvents([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (page <= 0 || pageSize <= 0)
            {
                return BadRequest("Page and pageSize must be positive integers.");
            }

            var cachedEvents = await _cacheService.GetCacheValueAsync<List<Event>>(AllEventsCacheKey);
            if (cachedEvents != null)
            {
                return Ok(cachedEvents.Select(e => new
                {
                    Id = e.Id,
                    Title = e.Title,
                    PhotoPath = e.PhotoPath,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    City = e.City,
                    OrganizationId = e.OrganizationId
                }).ToList());
            }

            var totalEvents = await _eventService.GetAllEventsAsync();
            await _cacheService.SetCacheValueAsync(AllEventsCacheKey, totalEvents, TimeSpan.FromMinutes(30));

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
            }).ToList();
        
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
            var cacheKey = $"{OrganizationEventsCacheKeyPrefix}{organizationCommonUserId}";

            var cachedEvents = await _cacheService.GetCacheValueAsync<List<Event>>(cacheKey);
            if (cachedEvents != null)
            {
                var cachedEventsToReturn = cachedEvents.Select(s => new
                {
                    s.Id,
                    s.Title,
                    s.City,
                    s.PhotoPath,
                    s.StartDate,
                    s.EndDate
                }).ToList();
                return Ok(cachedEventsToReturn);
            }

            var events = await _eventService.GetEventsForOrganizationAsync(organizationCommonUserId);

            await _cacheService.SetCacheValueAsync(cacheKey, events, TimeSpan.FromMinutes(30));

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
            var cacheKey = $"Event_{id}";
            var cachedEvent = await _cacheService.GetCacheValueAsync<Event>(cacheKey);
            if (cachedEvent != null)
            {
                var cachedEventToReturn = new
                {
                    Id = cachedEvent.Id,
                    OrganizationId = cachedEvent.OrganizationId,
                    OrganizationName = cachedEvent.Organization.Name,
                    Title = cachedEvent.Title,
                    PhotoPath = cachedEvent.PhotoPath,
                    StartDate = cachedEvent.StartDate,
                    EndDate = cachedEvent.EndDate,
                    City = cachedEvent.City,
                    Description = cachedEvent.Description
                };
                return Ok(cachedEventToReturn);
            }

            var eventItem = await _eventService.GetEventByIdAsync(id);
            if (eventItem == null)
            {
                return NotFound(new { Message = "Мероприятие не найдено" });
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

            await _cacheService.SetCacheValueAsync(cacheKey, eventItem, TimeSpan.FromMinutes(30));

            return Ok(eventToReturn);
        }

        [Authorize(Roles = "Organization")]
        [HttpPut("UpdateEvent/{id:int}")]
        public async Task<IActionResult> UpdateEvent([FromBody] UpdateEventDTO updatedEvent, int id)
        {
            await _eventService.UpdateEventAsync(updatedEvent, id);

            // Предотвратить неконсистентность данных (т.к. объект возможно в списке)
            await _cacheService.RemoveCacheValueAsync(AllEventsCacheKey);

            await _cacheService.RemoveCacheValueAsync($"Event_{id}");

            return Ok(new { Message = "Мероприятие успешно обновлено" });
        }

        [Authorize(Roles = "Organization")]
        [HttpDelete("Delete/{id:int}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var success = await _eventService.DeleteEventAsync(id);
            if (!success)
            {
                return NotFound(new { Message = "Мероприятие не найдено" });
            }

            // Предотвратить неконсистентность данных (т.к. объект возможно в списке)
            await _cacheService.RemoveCacheValueAsync(AllEventsCacheKey);
            await _cacheService.RemoveCacheValueAsync($"Event_{id}");

            return Ok(new { Message = "Мероприятие успешно удалено" });
        }
    }
}
