using TravelBilling.Domain.Invoices;
using TravelBilling.Domain.Subscriptions;

namespace TravelBilling.Domain.Services;

public interface ISubscriptionBillingDomainService
{
    Invoice ActivateAndGenerateFirstInvoice(Subscription subscription, DateTime activatedAtUtc);
    Invoice GenerateBillingCycleInvoice(Subscription subscription, DateTime nowUtc);
}
