using MakeVolunteerGreatAgain.Core.Entities;
using MakeVolunteerGreatAgain.Core.Repositories.DTO;

namespace MakeVolunteerGreatAgain.Core.Services;

public interface IEventService
{
    Task<IEnumerable<Event>> GetAllEventsAsync();
    Task<Event?> GetEventByIdAsync(int id);
    Task<Event> CreateEventAsync(EventCreateDTO eventModel, int commonUserId);
    Task<UpdateEventDTO> UpdateEventAsync(UpdateEventDTO eventModel, int id);
    Task<bool> DeleteEventAsync(int id);
   Task<IEnumerable<Event>> GetEventsForOrganizationAsync(int organizationCommonUserId);
}