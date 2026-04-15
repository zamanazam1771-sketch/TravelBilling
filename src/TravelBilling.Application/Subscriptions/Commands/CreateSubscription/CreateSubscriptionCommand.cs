using MediatR;
using TravelBilling.Application.Common;

namespace TravelBilling.Application.Subscriptions;

public sealed record CreateSubscriptionCommand(Guid CustomerId, string PlanName, decimal RecurringAmount, int BillingCycleDays) : IRequest<CommandResult<Guid>>;
