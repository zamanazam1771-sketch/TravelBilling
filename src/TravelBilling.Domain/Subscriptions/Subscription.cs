using TravelBilling.Domain.Common;
using TravelBilling.Domain.Events;

namespace TravelBilling.Domain.Subscriptions;

public sealed class Subscription : AggregateRoot
{
    private Subscription()
    {
    }

    private Subscription(Guid id, Guid customerId, string planName, decimal recurringAmount, int billingCycleDays)
    {
        if (string.IsNullOrWhiteSpace(planName))
        {
            throw new ArgumentException("Plan name is required.");
        }

        if (recurringAmount <= 0)
        {
            throw new ArgumentException("Recurring amount must be greater than zero.");
        }

        if (billingCycleDays <= 0)
        {
            throw new ArgumentException("Billing cycle must be greater than zero.");
        }

        Id = id;
        CustomerId = customerId;
        PlanName = planName.Trim();
        RecurringAmount = recurringAmount;
        BillingCycleDays = billingCycleDays;
        Status = SubscriptionStatus.Pending;
    }

    public Guid CustomerId { get; private set; }
    public string PlanName { get; private set; } = string.Empty;
    public decimal RecurringAmount { get; private set; }
    public int BillingCycleDays { get; private set; }
    public SubscriptionStatus Status { get; private set; }
    public DateTime? NextBillingDateUtc { get; private set; }

    public static Subscription Create(Guid customerId, string planName, decimal recurringAmount, int billingCycleDays) =>
        new(Guid.NewGuid(), customerId, planName, recurringAmount, billingCycleDays);

    public void Activate(DateTime activatedAtUtc)
    {
        if (Status == SubscriptionStatus.Active)
        {
            throw new InvalidOperationException("Subscription is already active.");
        }

        if (Status == SubscriptionStatus.Cancelled)
        {
            throw new InvalidOperationException("Cancelled subscription cannot be activated.");
        }

        Status = SubscriptionStatus.Active;
        NextBillingDateUtc = activatedAtUtc.AddDays(BillingCycleDays);
        AddDomainEvent(new SubscriptionActivated(Id, CustomerId, activatedAtUtc));
    }

    public void AdvanceBillingCycle(DateTime nowUtc)
    {
        if (Status != SubscriptionStatus.Active)
        {
            throw new InvalidOperationException("Only active subscriptions can generate invoices.");
        }
        if (NextBillingDateUtc is null || nowUtc < NextBillingDateUtc.Value)
        {
            throw new InvalidOperationException("Subscription is not yet due for billing.");
        }

        NextBillingDateUtc = NextBillingDateUtc.Value.AddDays(BillingCycleDays);
    }

    public void Cancel(DateTime cancelledAtUtc)
    {
        if (Status != SubscriptionStatus.Active)
        {
            throw new InvalidOperationException("Only active subscriptions can be cancelled.");
        }

        Status = SubscriptionStatus.Cancelled;
        NextBillingDateUtc = null;
    }
}
