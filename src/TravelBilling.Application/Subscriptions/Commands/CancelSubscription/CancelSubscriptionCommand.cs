using MediatR;

namespace TravelBilling.Application.Subscriptions;

public sealed record CancelSubscriptionCommand(Guid SubscriptionId) : IRequest<bool>;
