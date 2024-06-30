using System;
using System.Threading.Tasks;
using MakeVolunteerGreatAgain.Core.Repositories;
using MakeVolunteerGreatAgain.Persistence;

namespace MakeVolunteerGreatAgain.Infrastructure.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private VolunteerRepository _volunteerRepository;
        private OrganizationRepository _organizationRepository;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IVolunteerRepository VolunteerRepository => _volunteerRepository ??= new VolunteerRepository(_context);
        public IOrganizationRepository OrganizationRepository => _organizationRepository ??= new OrganizationRepository(_context);

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}