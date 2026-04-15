using MediatR;
using TravelBilling.Application.Common;

namespace TravelBilling.Application.Subscriptions;

public sealed record CancelSubscriptionCommand(Guid SubscriptionId) : IRequest<CommandResult>;
