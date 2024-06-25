namespace VolunteerProject.Services.Subscription;

using Microsoft.EntityFrameworkCore;
using VolunteerProject.DataBase;
using VolunteerProject.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

public interface ISubscriptionService
{
    Task<Subscription?> SubscribeAsync(int volunteerId, int organizationId);
    Task<Subscription?> SubscribeByInvitationAsync(int invitationId, int volunteerId);
    Task<IEnumerable<Subscription?>> GetSubscriptionsAsync();

}
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
}