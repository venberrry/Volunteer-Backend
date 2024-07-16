using MakeVolunteerGreatAgain.Core.Services;
using MakeVolunteerGreatAgain.Persistence;
using MakeVolunteerGreatAgain.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace MakeVolunteerGreatAgain.Infrastructure.Services;

public class SubscriptionService : ISubscriptionService
{
    private readonly ApplicationDbContext _context;

    public SubscriptionService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Subscription> SubscribeAsync(int volunteerCommonUserId, int organizationCommonUserId)
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

        // Создать новую подписку
        var subscription = new Subscription
        {
            // ОНО НЕ НАДО АРААРОАОАОАООАРАОААОАОА ПАМАГИТЕ ПАЖАЛАСТА XDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDD
            VolunteerId = volunteer.Id,
            OrganizationId = organization.Id,
            Status = SubscriptionStatus.Active.ToString(),
            Volunteer = volunteer,
            Organization = organization
        };

        // Добавить и сохранить подписку
        _context.Subscriptions.Add(subscription);
        await _context.SaveChangesAsync();
        return subscription;
    }

    public async Task<Subscription?> SubscribeByInvitationAsync(int invitationId, int volunteerCommonUserId)
    {
        var invitation = await _context.Invitations.FindAsync(invitationId);

         // Найти волонтера по CommonUserId
        var volunteer = await _context.Volunteers
            .FirstOrDefaultAsync(v => v.CommonUserId == volunteerCommonUserId) ?? throw new Exception("Volunteer not found");
            
        if (invitation == null || invitation.VolunteerId != volunteer.Id)
        {
            return null;
        }

        invitation.Status = InvitationStatus.Accepted.ToString();
        var invitations = _context.Invitations.Where(s => s.OrganizationId == invitation.OrganizationId);
        await invitations
            .Include(s => s.Volunteer)
            .Include(s => s.Organization)
            .LoadAsync();
        
        _context.Invitations.Update(invitation);
        await _context.SaveChangesAsync();

        return await SubscribeAsync(invitation.Volunteer.CommonUserId, invitation.Organization.CommonUserId);
    }

    public async Task<IEnumerable<Subscription>> GetSubscriptionsAsync(int organizationCommonUserId)
    {
        var organization = await _context.Organizations
            .FirstOrDefaultAsync(o => o.CommonUserId == organizationCommonUserId);
        if (organization == null)
        {
            throw new Exception("Organization not found");
        }

        var subscriptions = _context.Subscriptions.Where(s => s.OrganizationId == organization.Id);
        await subscriptions
            .Include(s => s.Volunteer)
            .Include(s => s.Organization)
            .LoadAsync();

        return subscriptions;
    }
    
    public async Task<IEnumerable<Subscription>> GetSubscriptionsByVolunteerAsync(int volunteerCommonUserId)
    {
        // Найти волонтера по CommonUserId
        var volunteer = await _context.Volunteers
            .FirstOrDefaultAsync(v => v.CommonUserId == volunteerCommonUserId);

        if (volunteer == null)
        {
            throw new Exception("Volunteer not found");
        }

        // Получить подписки для найденного волонтера
        var subscriptionsForVolunteer = _context.Subscriptions.Where(s => s.VolunteerId == volunteer.Id);

         await subscriptionsForVolunteer
            .Include(s => s.Volunteer)
            .Include(s => s.Organization)
            .LoadAsync();

        return subscriptionsForVolunteer;
    }
}