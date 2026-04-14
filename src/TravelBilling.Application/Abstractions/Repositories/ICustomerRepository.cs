using TravelBilling.Domain.Customers;

namespace TravelBilling.Application.Abstractions.Repositories;

public interface ICustomerRepository
{
    Task AddAsync(Customer customer, CancellationToken cancellationToken);
    Task<Customer?> GetAsync(Guid customerId, CancellationToken cancellationToken);
}
