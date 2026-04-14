using Microsoft.EntityFrameworkCore;
using TravelBilling.Application.Abstractions.Repositories;
using TravelBilling.Domain.Customers;

namespace TravelBilling.Infrastructure.Persistence.Repositories;

public sealed class CustomerRepository(TravelBillingDbContext dbContext) : ICustomerRepository
{
    public async Task AddAsync(Customer customer, CancellationToken cancellationToken) =>
        await dbContext.Customers.AddAsync(customer, cancellationToken);

    public async Task<Customer?> GetAsync(Guid customerId, CancellationToken cancellationToken) =>
        await dbContext.Customers.FirstOrDefaultAsync(x => x.Id == customerId, cancellationToken);
}
