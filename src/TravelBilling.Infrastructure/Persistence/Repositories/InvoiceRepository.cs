using Microsoft.EntityFrameworkCore;
using TravelBilling.Application.Abstractions.Repositories;
using TravelBilling.Domain.Invoices;

namespace TravelBilling.Infrastructure.Persistence.Repositories;

public sealed class InvoiceRepository(TravelBillingDbContext dbContext) : IInvoiceRepository
{
    public async Task AddAsync(Invoice invoice, CancellationToken cancellationToken) =>
        await dbContext.Invoices.AddAsync(invoice, cancellationToken);

    public async Task<Invoice?> GetAsync(Guid invoiceId, CancellationToken cancellationToken) =>
        await dbContext.Invoices.FirstOrDefaultAsync(x => x.Id == invoiceId, cancellationToken);

    public async Task<IReadOnlyList<Invoice>> GetByCustomerAsync(Guid customerId, CancellationToken cancellationToken) =>
        await dbContext.Invoices
            .Where(x => x.CustomerId == customerId)
            .OrderByDescending(x => x.IssuedOnUtc)
            .ToListAsync(cancellationToken);
}
