namespace MakeVolunteerGreatAgain.Core.Repositories
{
    public interface IOrganizationRepository
    {
        Task<Entities.Organization> GetByIdAsync(int id);
        Task AddAsync(Entities.Organization organization);
        Task SaveChangesAsync();
    }
}