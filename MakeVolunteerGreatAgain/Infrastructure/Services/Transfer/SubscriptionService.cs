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

    public async Task<Subscription?> SubscribeAsync(int volunteerId, int organizationId)
    {
        var subscription = new Subscription
        {
            VolunteerId = volunteerId,
            OrganizationId = organizationId,
            Status = "Pending"
        };

        _context.Subscriptions.Add(subscription);
        await _context.SaveChangesAsync();
        return subscription;
    }

    public async Task<Subscription?> SubscribeByInvitationAsync(int invitationId, int volunteerId)
    {
        var invitation = await _context.Invitations.FindAsync(invitationId);
        
        if (invitation == null || invitation.VolunteerId != volunteerId)
        {
            return null;
        }

        return await SubscribeAsync(invitation.VolunteerId, invitation.OrganizationId);
    }

    public async Task<IEnumerable<Subscription?>> GetSubscriptionsAsync()
    {
        return await _context.Subscriptions.ToListAsync();
    }
    
    public async Task<IEnumerable<Subscription>> GetSubscriptionsByVolunteerAsync(int volunteerId) // Реализация нового метода
    {
        return await _context.Subscriptions.Where(s => s.VolunteerId == volunteerId).ToListAsync();
    }
}