namespace VolunteerProject.Services.Subscription;

using Microsoft.EntityFrameworkCore;
using VolunteerProject.DataBase;
using VolunteerProject.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using VolunteerProject.Services.Subscription;
// using VolunteerProject.Services.Email;

public class SubscriptionService : ISubscriptionService
{
    private readonly ApplicationDbContext _context;
    // private readonly IEmailService _emailService;

    public SubscriptionService(ApplicationDbContext context)
    // public SubscriptionService(ApplicationDbContext context, IEmailService emailService)
    {
        _context = context;
        // _emailService = emailService;
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