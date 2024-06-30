using MakeVolunteerGreatAgain.Core.Entities;

namespace MakeVolunteerGreatAgain.Core.Repositories.Volunteer;

public interface ISubscriptionRepository
{
    Task<IEnumerable<Subscription>> GetAllSubscriptionsAsync();
    Task<Subscription> GetSubscriptionByIdAsync(int id);
    Task<Subscription> CreateSubscriptionAsync(Subscription subscription);
    Task<Application> UpdateSubscriptionAsync(int id, Subscription updatedSubscription);
    Task<Subscription> DeleteSubscriptionAsync(int id);
}