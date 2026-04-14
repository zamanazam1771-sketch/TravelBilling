using TravelBilling.Domain.Common;

namespace TravelBilling.Domain.Events;

public sealed record PaymentReceived(Guid InvoiceId, Guid CustomerId, DateTime OccurredOnUtc) : IDomainEvent;
