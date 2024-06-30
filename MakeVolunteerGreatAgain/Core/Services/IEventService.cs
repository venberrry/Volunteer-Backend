using MakeVolunteerGreatAgain.Core.Entities;

namespace MakeVolunteerGreatAgain.Core.Services;

public interface IEventService
{
    Task<IEnumerable<Event>> GetAllEventsAsync();
    Task<Event?> GetEventByIdAsync(int id);
    Task<Event> CreateEventAsync(Event eventObj);
    Task<Event> UpdateEventAsync(int id, Event updatedEvent);
    Task<bool> DeleteEventAsync(int id);
}