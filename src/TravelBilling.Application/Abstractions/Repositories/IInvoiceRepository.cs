using TravelBilling.Domain.Invoices;

namespace TravelBilling.Application.Abstractions.Repositories;

public interface IInvoiceRepository
{
    Task AddAsync(Invoice invoice, CancellationToken cancellationToken);
    Task<Invoice?> GetAsync(Guid invoiceId, CancellationToken cancellationToken);
    Task<IReadOnlyList<Invoice>> GetByCustomerAsync(Guid customerId, CancellationToken cancellationToken);
}
