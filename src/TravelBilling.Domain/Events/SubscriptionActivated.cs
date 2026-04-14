using TravelBilling.Domain.Common;

namespace TravelBilling.Domain.Events;

public sealed record SubscriptionActivated(Guid SubscriptionId, Guid CustomerId, DateTime OccurredOnUtc) : IDomainEvent;
