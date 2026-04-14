using MediatR;
using Microsoft.Extensions.Logging;
using TravelBilling.Domain.Events;

namespace TravelBilling.Application.Events;

public sealed class PaymentReceivedHandler(ILogger<PaymentReceivedHandler> logger)
    : INotificationHandler<PaymentReceived>
{
    public Task Handle(PaymentReceived notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Domain event: {EventName} InvoiceId={InvoiceId} CustomerId={CustomerId} OccurredOnUtc={OccurredOnUtc}",
            nameof(PaymentReceived),
            notification.InvoiceId,
            notification.CustomerId,
            notification.OccurredOnUtc);

        return Task.CompletedTask;
    }
}
