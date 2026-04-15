namespace TravelBilling.Api.Contracts.Subscriptions;

public sealed record CreateSubscriptionRequest(Guid CustomerId, string PlanName, decimal RecurringAmount, int BillingCycleDays);
