using TravelBilling.Domain.Common;

namespace TravelBilling.Domain.Events;

public sealed record InvoiceGenerated(Guid InvoiceId, Guid SubscriptionId, Guid CustomerId, DateTime OccurredOnUtc) : IDomainEvent;
