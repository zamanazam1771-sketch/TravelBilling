using Microsoft.EntityFrameworkCore;
using TravelBilling.Application.Abstractions.Repositories;
using TravelBilling.Domain.Subscriptions;

namespace TravelBilling.Infrastructure.Persistence.Repositories;

public sealed class SubscriptionRepository(TravelBillingDbContext dbContext) : ISubscriptionRepository
{
    public async Task AddAsync(Subscription subscription, CancellationToken cancellationToken) =>
        await dbContext.Subscriptions.AddAsync(subscription, cancellationToken);

    public async Task<Subscription?> GetAsync(Guid subscriptionId, CancellationToken cancellationToken) =>
        await dbContext.Subscriptions.FirstOrDefaultAsync(x => x.Id == subscriptionId, cancellationToken);

    public async Task<IReadOnlyList<Subscription>> GetDueActiveSubscriptionsAsync(DateTime nowUtc, CancellationToken cancellationToken) =>
        await dbContext.Subscriptions
            .Where(x => x.Status == SubscriptionStatus.Active && x.NextBillingDateUtc <= nowUtc)
            .ToListAsync(cancellationToken);
}
