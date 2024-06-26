using Microsoft.EntityFrameworkCore;
using VolunteerProject.DataBase;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VolunteerProject.Services.Invitation;
using VolunteerProject.Models;

public class InvitationService : IInvitationService
{
    private readonly ApplicationDbContext _context;

    public InvitationService(ApplicationDbContext context)
    {
        _context = context;
    }

    //Получение приглашения по id
    public async Task<Invitation?> GetInvitationByIdAsync(int id)
    {
        return await _context.Invitations.FindAsync(id);
    }

    //Получение всех приглашений
    public async Task<IEnumerable<Invitation?>> GetAllInvitationsAsync()
    {
        return await _context.Invitations.ToListAsync();
    }

    //Создание нового приглашения
    public async Task<Invitation> CreateInvitationAsync(Invitation invitation)
    {
        _context.Invitations.Add(invitation);
        await _context.SaveChangesAsync();
        return invitation;
    }

    //Обновление приглашения
    public async Task<Invitation> UpdateInvitationAsync(int id, Invitation updatedInvitation)
    {
        var invitation = await _context.Invitations.FindAsync(id);

        if (invitation == null)
        {
            throw new Exception("Invitation not found");
        }

        invitation.VolunteerId = updatedInvitation.VolunteerId;

        await _context.SaveChangesAsync();
        return invitation;
    }

    //Удаление приглашения
    public async Task<Invitation?> DeleteInvitationAsync(int id)
    {
        var invitation = await _context.Invitations.FindAsync(id);
        if (invitation == null)
        {
            return null;
        }

        _context.Invitations.Remove(invitation);
        await _context.SaveChangesAsync();
        return invitation;
    }
}