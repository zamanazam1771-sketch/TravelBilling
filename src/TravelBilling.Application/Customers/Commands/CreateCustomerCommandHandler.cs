using MediatR;
using TravelBilling.Application.Abstractions;
using TravelBilling.Application.Abstractions.Repositories;
using TravelBilling.Domain.Customers;

namespace TravelBilling.Application.Customers;

public sealed class CreateCustomerCommandHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<CreateCustomerCommand, Guid>
{
    public async Task<Guid> Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
    {
        var customer = Customer.Create(command.Name, command.Email);
        await customerRepository.AddAsync(customer, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return customer.Id;
    }
}
