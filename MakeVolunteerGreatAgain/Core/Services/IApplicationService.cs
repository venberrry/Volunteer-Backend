using MakeVolunteerGreatAgain.Core.Entities;
using MakeVolunteerGreatAgain.Core.Repositories.DTO;

namespace MakeVolunteerGreatAgain.Core.Services;

public interface IApplicationService
{
    Task<Application?> GetApplicationByIdAsync(int id);
    Task<Application> ApplyAsync(ApplicationCreateDTO applicationObj, int commonUserId, int eventId);
    Task<bool> UnapplyAsync(int applicationId);
    Task<IEnumerable<Application>> GetApplicationsByEventIdAsync(int eventId);
    Task<IEnumerable<Application>> GetAcceptedApplicationsByEventIdAsync(int eventId);
    Task <Application?> AcceptAplicationAsync(int id);
    Task <Application?> RejectAplicationAsync(int id);
    Task<bool> HasAppliedAsync(int volunteerCommonUserId, int eventId);
}