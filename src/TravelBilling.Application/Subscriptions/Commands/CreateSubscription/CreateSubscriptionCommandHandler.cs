using MediatR;
using TravelBilling.Application.Abstractions;
using TravelBilling.Application.Abstractions.Repositories;
using TravelBilling.Domain.Invoices;
using TravelBilling.Domain.Subscriptions;

namespace TravelBilling.Application.Subscriptions;

public sealed class CreateSubscriptionCommandHandler(
    ICustomerRepository customerRepository,
    ISubscriptionRepository subscriptionRepository,
    IInvoiceRepository invoiceRepository,
    IUnitOfWork unitOfWork,
    IClock clock)
    : IRequestHandler<CreateSubscriptionCommand, Guid>
{
    public async Task<Guid> Handle(CreateSubscriptionCommand command, CancellationToken cancellationToken)
    {
        var customer = await customerRepository.GetAsync(command.CustomerId, cancellationToken);
        if (customer is null)
        {
            throw new InvalidOperationException("Customer was not found.");
        }

        var subscription = Subscription.Create(command.CustomerId, command.PlanName, command.RecurringAmount, command.BillingCycleDays);
        var now = clock.UtcNow;
        subscription.Activate(now);
        var firstInvoice = Invoice.Generate(subscription.CustomerId, subscription.Id, subscription.RecurringAmount, now);

        await subscriptionRepository.AddAsync(subscription, cancellationToken);
        await invoiceRepository.AddAsync(firstInvoice, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return subscription.Id;
    }
}
