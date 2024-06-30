using MakeVolunteerGreatAgain.Core.Entities;
using MakeVolunteerGreatAgain.Core.Repositories;
using MakeVolunteerGreatAgain.Persistence;

namespace MakeVolunteerGreatAgain.Infrastructure.Services
{
    public class VolunteerRepository : IVolunteerRepository
    {
        private readonly ApplicationDbContext _context;

        public VolunteerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Volunteer> GetByIdAsync(int id)
        {
            return await _context.Volunteers.FindAsync(id);
        }

        public async Task AddAsync(Volunteer volunteer)
        {
            await _context.Volunteers.AddAsync(volunteer);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}