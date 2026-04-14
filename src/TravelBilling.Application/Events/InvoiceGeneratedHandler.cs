using MediatR;
using Microsoft.Extensions.Logging;
using TravelBilling.Domain.Events;

namespace TravelBilling.Application.Events;

public sealed class InvoiceGeneratedHandler(ILogger<InvoiceGeneratedHandler> logger)
    : INotificationHandler<InvoiceGenerated>
{
    public Task Handle(InvoiceGenerated notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Domain event: {EventName} InvoiceId={InvoiceId} SubscriptionId={SubscriptionId} CustomerId={CustomerId} OccurredOnUtc={OccurredOnUtc}",
            nameof(InvoiceGenerated),
            notification.InvoiceId,
            notification.SubscriptionId,
            notification.CustomerId,
            notification.OccurredOnUtc);

        return Task.CompletedTask;
    }
}
