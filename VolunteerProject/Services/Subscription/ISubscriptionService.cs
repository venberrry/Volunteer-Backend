namespace VolunteerProject.Services.Subscription;
using VolunteerProject.Models;
public interface ISubscriptionService
{
    Task<Subscription?> SubscribeAsync(int volunteerId, int organizationId);
    Task<Subscription?> SubscribeByInvitationAsync(int invitationId, int volunteerId);
    Task<IEnumerable<Subscription?>> GetSubscriptionsAsync();

}