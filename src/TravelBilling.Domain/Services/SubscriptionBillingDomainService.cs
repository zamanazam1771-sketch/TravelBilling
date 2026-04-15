using TravelBilling.Domain.Invoices;
using TravelBilling.Domain.Subscriptions;

namespace TravelBilling.Domain.Services;

public sealed class SubscriptionBillingDomainService : ISubscriptionBillingDomainService
{
    public Invoice ActivateAndGenerateFirstInvoice(Subscription subscription, DateTime activatedAtUtc)
    {
        subscription.Activate(activatedAtUtc);
        return Invoice.Generate(subscription.CustomerId, subscription.Id, subscription.RecurringAmount, activatedAtUtc);
    }

    public Invoice GenerateBillingCycleInvoice(Subscription subscription, DateTime nowUtc)
    {
        subscription.AdvanceBillingCycle(nowUtc);
        return Invoice.Generate(subscription.CustomerId, subscription.Id, subscription.RecurringAmount, nowUtc);
    }
}
