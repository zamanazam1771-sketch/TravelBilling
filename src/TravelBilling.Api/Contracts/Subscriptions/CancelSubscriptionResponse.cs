namespace TravelBilling.Api.Contracts.Subscriptions;

public sealed record CancelSubscriptionResponse(Guid SubscriptionId, string Status);
