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

    public async Task<Subscription?> SubscribeAsync(int volunteerCommonUserId, int organizationCommonUserId)
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
            Status = "Pending",
            Volunteer = volunteer,
            Organization = organization
        };

        // Добавить и сохранить подписку
        _context.Subscriptions.Add(subscription);
        await _context.SaveChangesAsync();
        return subscription;
    }

    public async Task<Subscription> SubscribeByInvitationAsync(int invitationId, int volunteerId)
    {
        var invitation = await _context.Invitations.FindAsync(invitationId);
        
        if (invitation == null || invitation.VolunteerId != volunteerId)
        {
            return null;
        }

        return await SubscribeAsync(invitation.VolunteerId, invitation.OrganizationId);
    }

    public async Task<IEnumerable<Subscription>> GetSubscriptionsAsync(int organizationCommonUserId)
    {
        var organization = await _context.Organizations
            .FirstOrDefaultAsync(o => o.CommonUserId == organizationCommonUserId);
        if (organization == null)
        {
            throw new Exception("Organization not found");
        }

        return await _context.Subscriptions
            .Where(s => s.OrganizationId == organization.Id)
            .ToListAsync();
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
        return await _context.Subscriptions
            .Where(s => s.VolunteerId == volunteer.Id)
            .ToListAsync();
    }
}