using MakeVolunteerGreatAgain.Core.Entities;

namespace MakeVolunteerGreatAgain.Core.Repositories.Volunteer;

public interface IApplicationRepository
{
    Task<IEnumerable<Application>> GetAllApplicationsAsync();
    Task<Application> GetApplicationByIdAsync(int id);
    Task<Application> CreateApplicationAsync(Application invitation);
    Task<Application> UpdateApplicationAsync(int id, Application updatedApplication);
    Task<Application> DeleteApplicationAsync(int id);
}