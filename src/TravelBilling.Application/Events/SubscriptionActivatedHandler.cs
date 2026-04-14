using MediatR;
using Microsoft.Extensions.Logging;
using TravelBilling.Domain.Events;

namespace TravelBilling.Application.Events;

public sealed class SubscriptionActivatedHandler(ILogger<SubscriptionActivatedHandler> logger)
    : INotificationHandler<SubscriptionActivated>
{
    public Task Handle(SubscriptionActivated notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Domain event: {EventName} SubscriptionId={SubscriptionId} CustomerId={CustomerId} OccurredOnUtc={OccurredOnUtc}",
            nameof(SubscriptionActivated),
            notification.SubscriptionId,
            notification.CustomerId,
            notification.OccurredOnUtc);

        return Task.CompletedTask;
    }
}
