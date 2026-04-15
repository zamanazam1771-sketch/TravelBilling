using MediatR;
using TravelBilling.Application.Abstractions;
using TravelBilling.Application.Abstractions.Repositories;
using TravelBilling.Application.Common;

namespace TravelBilling.Application.Subscriptions;

public sealed class CancelSubscriptionCommandHandler(
    ISubscriptionRepository subscriptionRepository,
    IUnitOfWork unitOfWork,
    IClock clock)
    : IRequestHandler<CancelSubscriptionCommand, CommandResult>
{
    public async Task<CommandResult> Handle(CancelSubscriptionCommand command, CancellationToken cancellationToken)
    {
        var subscription = await subscriptionRepository.GetAsync(command.SubscriptionId, cancellationToken);
        if (subscription is null)
        {
            return CommandResult.Failure(ApplicationError.NotFound("Subscription was not found."));
        }

        try
        {
            subscription.Cancel(clock.UtcNow);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return CommandResult.Success();
        }
        catch (InvalidOperationException exception)
        {
            return CommandResult.Failure(ApplicationError.Conflict(exception.Message));
        }
    }
}
