using MediatR;
using TravelBilling.Application.Common;

namespace TravelBilling.Application.Customers;

public sealed record CreateCustomerCommand(string Name, string Email) : IRequest<CommandResult<Guid>>;
