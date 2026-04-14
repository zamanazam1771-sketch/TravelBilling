using MediatR;
using TravelBilling.Application.Abstractions;
using TravelBilling.Application.Abstractions.Repositories;

namespace TravelBilling.Application.Subscriptions;

public sealed class CancelSubscriptionCommandHandler(
    ISubscriptionRepository subscriptionRepository,
    IUnitOfWork unitOfWork,
    IClock clock)
    : IRequestHandler<CancelSubscriptionCommand, bool>
{
    public async Task<bool> Handle(CancelSubscriptionCommand command, CancellationToken cancellationToken)
    {
        var subscription = await subscriptionRepository.GetAsync(command.SubscriptionId, cancellationToken);
        if (subscription is null)
        {
            return false;
        }

        subscription.Cancel(clock.UtcNow);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
