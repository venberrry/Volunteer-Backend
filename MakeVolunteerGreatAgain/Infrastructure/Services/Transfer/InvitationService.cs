using MakeVolunteerGreatAgain.Core.Services;
using MakeVolunteerGreatAgain.Persistence;
using MakeVolunteerGreatAgain.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace MakeVolunteerGreatAgain.Infrastructure.Services;

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
    public async Task<IEnumerable<Invitation?>> GetAllInvitationsAsync(int organizationCommonUserId)
    {
        var organization = await _context.Organizations
            .FirstOrDefaultAsync(o => o.CommonUserId == organizationCommonUserId);
        if (organization == null)
        {
            throw new Exception("Organization not found");
        }

        var invitations =  _context.Invitations.Where(s => s.OrganizationId == organization.Id);

        await invitations
            .Include(i => i.Volunteer)
            .Include(i => i.Organization)
            .LoadAsync();   
        return invitations;   
    }
    
    // Создание нового приглашения
    public async Task<Invitation> CreateInvitationAsync(int volunteerCommonUserId, int organizationCommonUserId)
    {
        // Найти волонтера по CommonUserId
        var volunteer = await _context.Volunteers
            .FirstOrDefaultAsync(v => v.CommonUserId == volunteerCommonUserId);
        if (volunteer == null)
        {
            throw new Exception("Volunteer not found");
        }

        // Найти организацию по CommonUserId
        var organization = await _context.Organizations
            .FirstOrDefaultAsync(o => o.CommonUserId == organizationCommonUserId);
        if (organization == null)
        {
            throw new Exception("Organization not found");
        }
    
        var invitation = new Invitation
        {
            OrganizationId = organization.Id,
            Organization = organization,
            VolunteerId = volunteer.Id,
            Volunteer = volunteer,
            Status = InvitationStatus.Pending.ToString()
        };
        await _context.Invitations.AddAsync(invitation);
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

    public async Task<IEnumerable<Invitation>> GetAllInvitationsForVolunteerAsync(int volunteerCommonUserId)
    {
        var volunteer = await _context.Volunteers
            .FirstOrDefaultAsync(v => v.CommonUserId == volunteerCommonUserId) ?? throw new Exception("Volunteer not found");
        
        var invitationsForVolunteer = _context.Invitations.Where(i => i.VolunteerId == volunteer.Id);
        await invitationsForVolunteer
            .Include(i => i.Volunteer)
            .Include(i => i.Organization)
            .LoadAsync();
            
        return invitationsForVolunteer;
    }
}