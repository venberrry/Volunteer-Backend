using MakeVolunteerGreatAgain.Core.Entities;
using MakeVolunteerGreatAgain.Core.Repositories;
using MakeVolunteerGreatAgain.Persistence;

namespace MakeVolunteerGreatAgain.Infrastructure.Services
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly ApplicationDbContext _context;

        public OrganizationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Organization> GetByIdAsync(int id)
        {
            return await _context.Organizations.FindAsync(id);
        }

        public async Task AddAsync(Organization organization)
        {
            await _context.Organizations.AddAsync(organization);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}