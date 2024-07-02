using MakeVolunteerGreatAgain.Core.Services;
using MakeVolunteerGreatAgain.Persistence;
using MakeVolunteerGreatAgain.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace MakeVolunteerGreatAgain.Infrastructure.Services;

public class EventService : IEventService
{
    private readonly ApplicationDbContext _context;

    public EventService(ApplicationDbContext context) {
        _context = context;
    }

    public async Task<IEnumerable<Event>> GetAllEventsAsync()
    {
        return await _context.Events.ToListAsync();
    }

    public async Task<Event?> GetEventByIdAsync(int id)
    {
        return await _context.Events.FindAsync(id);
    }

    public async Task<Event> CreateEventAsync(Event eventObj)
    {
        _context.Events.Add(eventObj);
        await _context.SaveChangesAsync();
        return eventObj;
    }

    public async Task<Event> UpdateEventAsync(int id, Event updatedEvent)
    {
        var existingEvent = await _context.Events.FindAsync(id);
        if (existingEvent == null)
        {
            return null;
        }

        existingEvent.Title = updatedEvent.Title;
        existingEvent.PhotoPath = updatedEvent.PhotoPath;
        existingEvent.StartDate = updatedEvent.StartDate;
        existingEvent.EndDate = updatedEvent.EndDate;
        existingEvent.City = updatedEvent.City;
        existingEvent.Description = updatedEvent.Description;
        existingEvent.OrganizationId = updatedEvent.OrganizationId;

        await _context.SaveChangesAsync();
        return existingEvent;
    }

    public async Task<bool> DeleteEventAsync(int id)
    {
        var eventToDelete = await _context.Events.FindAsync(id);
        if (eventToDelete == null)
        {
            return false;
        }

        _context.Events.Remove(eventToDelete);
        await _context.SaveChangesAsync();
        return true;
    }
}