using MediatR;

namespace TravelBilling.Application.Customers;

public sealed record CreateCustomerCommand(string Name, string Email) : IRequest<Guid>;
