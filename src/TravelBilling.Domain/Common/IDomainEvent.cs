using MediatR;

namespace TravelBilling.Domain.Common;

public interface IDomainEvent : INotification
{
    DateTime OccurredOnUtc { get; }
}
