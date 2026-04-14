using MediatR;
using TravelBilling.Application.Abstractions;
using TravelBilling.Domain.Common;

namespace TravelBilling.Infrastructure.Persistence;

public sealed class UnitOfWork(TravelBillingDbContext dbContext, IPublisher publisher) : IUnitOfWork
{
    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        var domainEvents = dbContext.ChangeTracker
            .Entries<AggregateRoot>()
            .Select(entry => entry.Entity)
            .SelectMany(aggregate => aggregate.DomainEvents)
            .ToList();

        await dbContext.SaveChangesAsync(cancellationToken);

        foreach (var aggregate in dbContext.ChangeTracker.Entries<AggregateRoot>().Select(entry => entry.Entity))
        {
            aggregate.ClearDomainEvents();
        }

        foreach (var domainEvent in domainEvents)
        {
            await publisher.Publish(domainEvent, cancellationToken);
        }
    }
}
