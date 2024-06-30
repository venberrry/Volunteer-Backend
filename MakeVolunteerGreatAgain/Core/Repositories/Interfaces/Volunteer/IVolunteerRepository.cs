namespace MakeVolunteerGreatAgain.Core.Repositories
{
    public interface IVolunteerRepository
    {
        Task<Entities.Volunteer> GetByIdAsync(int id);
        Task AddAsync(Entities.Volunteer volunteer);
        Task SaveChangesAsync();
    }
}