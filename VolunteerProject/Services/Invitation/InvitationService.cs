using Microsoft.EntityFrameworkCore;
using VolunteerProject.DataBase;
using VolunteerProject.Models;

namespace VolunteerProject.Services
{
    public interface IInvitationService
    {
        Task<Invitation> CreateInvitationAsync(Invitation invitation);
        Task<Invitation> UpdateInvitationAsync(int id, Invitation updatedInvitation);
        Task<Invitation> DeleteInvitationAsync(int id);
        Task<IEnumerable<Invitation>> GetAllInvitationsAsync();
        Task<Invitation> GetInvitationByIdAsync(int id);
    }

    public class InvitationService : IInvitationService
    {
        private readonly ApplicationDbContext _context;
        public InvitationService(ApplicationDbContext context)
        {
            _context = context;
        }

        //Получение приглашения по id
        public async Task<Invitation> GetInvitationByIdAsync(int id)
        {
            return await _context.Invitation.FindAsync(id);
        }

        //Получение всех приглашений
        public async Task<IEnumerable<Invitation>> GetAllInvitationsAsync()
        {
            return await _context.Invitation.ToListAsync();
        }

        //Создание нового приглашения
        public async Task<Invitation> CreateInvitationAsync(Invitation invitation)
        {
            _context.Invitation.Add(invitation);
            await _context.SaveChangesAsync();
            return invitation;
        }

        //Обновление приглашения
        public async Task<Invitation> UpdateInvitationAsync(int id, Invitation updatedInvitation)
        {
            var Invitation = await _context.Invitation.FindAsync(id);

            if (Invitation == null)
            {
                return null;
            }

            Invitation.VolunteerId = updatedInvitation.VolunteerId;

            _context.SaveChanges();
            return Invitation;
        }

        //Удаление приглашения
        public async Task<Invitation> DeleteInvitationAsync(int id)
        {
            var Invitation = await _context.Invitation.FindAsync(id);
            if (Invitation == null)
            {
                return null;
            }

            _context.Invitation.Remove(Invitation);
            await _context.SaveChangesAsync();
            return Invitation;
        }
    }
}
