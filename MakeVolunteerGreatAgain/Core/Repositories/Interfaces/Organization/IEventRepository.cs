using MakeVolunteerGreatAgain.Core.Entities;

namespace MakeVolunteerGreatAgain.Core.Repositories.Organization;

public interface IEventRepository
{
    Task<IEnumerable<Event>> GetAllEventsAsync();
    Task<Event> GetEventByIdAsync(int id);
    Task<Event> CreateEventAsync(Event eventObj);
    Task<Event> UpdateEventAsync(int id, Event updatedEvent);
    Task<bool> DeleteEventAsync(int id);
}