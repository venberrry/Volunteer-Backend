using MakeVolunteerGreatAgain.Core.Entities;

namespace MakeVolunteerGreatAgain.Core.Services;

public interface ISubscriptionService
{
    Task<Subscription> SubscribeAsync(int volunteerId, int organizationId);
    Task<Subscription?> SubscribeByInvitationAsync(int invitationId, int volunteerId);
    Task<IEnumerable<Subscription>> GetSubscriptionsAsync(int organizationCommonUserId);
    Task<IEnumerable<Subscription>> GetSubscriptionsByVolunteerAsync(int volunteerId); //Новый метод
}