using MediatR;
using TravelBilling.Application.Abstractions;
using TravelBilling.Application.Abstractions.Repositories;
using TravelBilling.Application.Common;
using TravelBilling.Domain.Services;
using TravelBilling.Domain.Subscriptions;

namespace TravelBilling.Application.Subscriptions;

public sealed class CreateSubscriptionCommandHandler(
    ICustomerRepository customerRepository,
    ISubscriptionRepository subscriptionRepository,
    IInvoiceRepository invoiceRepository,
    IUnitOfWork unitOfWork,
    IClock clock,
    ISubscriptionBillingDomainService subscriptionBillingDomainService)
    : IRequestHandler<CreateSubscriptionCommand, CommandResult<Guid>>
{
    public async Task<CommandResult<Guid>> Handle(CreateSubscriptionCommand command, CancellationToken cancellationToken)
    {
        var customer = await customerRepository.GetAsync(command.CustomerId, cancellationToken);
        if (customer is null)
        {
            return CommandResult<Guid>.Failure(ApplicationError.NotFound("Customer was not found."));
        }

        try
        {
            var subscription = Subscription.Create(command.CustomerId, command.PlanName, command.RecurringAmount, command.BillingCycleDays);
            var now = clock.UtcNow;
            var firstInvoice = subscriptionBillingDomainService.ActivateAndGenerateFirstInvoice(subscription, now);

            await subscriptionRepository.AddAsync(subscription, cancellationToken);
            await invoiceRepository.AddAsync(firstInvoice, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return CommandResult<Guid>.Success(subscription.Id);
        }
        catch (ArgumentException exception)
        {
            return CommandResult<Guid>.Failure(ApplicationError.Validation(exception.Message));
        }
        catch (InvalidOperationException exception)
        {
            return CommandResult<Guid>.Failure(ApplicationError.Conflict(exception.Message));
        }
    }
}
