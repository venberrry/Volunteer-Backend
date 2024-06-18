namespace VolunteerProject.Controllers.Events;
using Microsoft.AspNetCore.Mvc;
using VolunteerProject.Models;
using VolunteerProject.Services.Events;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api")]
[ApiController]
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

    [HttpGet("GetById{id}")]
    public async Task<ActionResult<Event>> GetEventById(int id)
    {
        var eventItem = await _eventService.GetEventByIdAsync(id);
        if (eventItem == null)
        {
            return NotFound();
        }
        return Ok(eventItem);
    }

    [HttpPost("CreateEvent")]
    public async Task<ActionResult> CreateEvent([FromBody] CreateEventModel eventModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Создание объекта Event на основе данных из eventModel
        var newEvent = new Event
        {
            OrganizationId = eventModel.OrganizationId,
            Title = eventModel.Title,
            StartDate = eventModel.StartDate,
            EndDate = eventModel.EndDate,
            City = eventModel.City,
            Description = eventModel.Description
        
        };
        
        var createdEvent = await _eventService.CreateEventAsync(newEvent);

        return Ok(createdEvent);
    }

    [HttpPut("UpdateEvent{id}")]
    public async Task<IActionResult> UpdateEvent(int id, Event updatedEvent)
    {
        var eventItem = await _eventService.UpdateEventAsync(id, updatedEvent);
        if (eventItem == null)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpDelete("Delete{id}")]
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