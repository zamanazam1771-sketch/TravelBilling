using MediatR;
using TravelBilling.Application.Abstractions;
using TravelBilling.Application.Abstractions.Repositories;
using TravelBilling.Application.Common;
using TravelBilling.Domain.Customers;

namespace TravelBilling.Application.Customers;

public sealed class CreateCustomerCommandHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<CreateCustomerCommand, CommandResult<Guid>>
{
    public async Task<CommandResult<Guid>> Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var customer = Customer.Create(command.Name, command.Email);
            await customerRepository.AddAsync(customer, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return CommandResult<Guid>.Success(customer.Id);
        }
        catch (ArgumentException exception)
        {
            return CommandResult<Guid>.Failure(ApplicationError.Validation(exception.Message));
        }
    }
}
