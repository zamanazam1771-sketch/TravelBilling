using MediatR;

namespace TravelBilling.Application.Subscriptions;

public sealed record GenerateBillingCycleInvoicesCommand : IRequest<int>;
