using MediatR;
using TravelBilling.Application.Abstractions;
using TravelBilling.Application.Abstractions.Repositories;
using TravelBilling.Domain.Invoices;

namespace TravelBilling.Application.Subscriptions;

public sealed class GenerateBillingCycleInvoicesCommandHandler(
    ISubscriptionRepository subscriptionRepository,
    IInvoiceRepository invoiceRepository,
    IUnitOfWork unitOfWork,
    IClock clock)
    : IRequestHandler<GenerateBillingCycleInvoicesCommand, int>
{
    public async Task<int> Handle(GenerateBillingCycleInvoicesCommand command, CancellationToken cancellationToken)
    {
        var now = clock.UtcNow;
        var dueSubscriptions = await subscriptionRepository.GetDueActiveSubscriptionsAsync(now, cancellationToken);
        var created = 0;

        foreach (var subscription in dueSubscriptions)
        {
            subscription.AdvanceBillingCycle(now);
            var invoice = Invoice.Generate(subscription.CustomerId, subscription.Id, subscription.RecurringAmount, now);
            await invoiceRepository.AddAsync(invoice, cancellationToken);
            created++;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return created;
    }
}
