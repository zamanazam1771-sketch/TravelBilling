using TravelBilling.Domain.Subscriptions;

namespace TravelBilling.Application.Abstractions.Repositories;

public interface ISubscriptionRepository
{
    Task AddAsync(Subscription subscription, CancellationToken cancellationToken);
    Task<Subscription?> GetAsync(Guid subscriptionId, CancellationToken cancellationToken);
    Task<IReadOnlyList<Subscription>> GetDueActiveSubscriptionsAsync(DateTime nowUtc, CancellationToken cancellationToken);
}
