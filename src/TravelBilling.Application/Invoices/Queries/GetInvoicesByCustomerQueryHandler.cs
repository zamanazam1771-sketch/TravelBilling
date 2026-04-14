using MediatR;
using TravelBilling.Application.Abstractions.Repositories;

namespace TravelBilling.Application.Invoices;

public sealed class GetInvoicesByCustomerQueryHandler(IInvoiceRepository invoiceRepository)
    : IRequestHandler<GetInvoicesByCustomerQuery, IReadOnlyList<InvoiceDto>>
{
    public async Task<IReadOnlyList<InvoiceDto>> Handle(GetInvoicesByCustomerQuery query, CancellationToken cancellationToken)
    {
        var invoices = await invoiceRepository.GetByCustomerAsync(query.CustomerId, cancellationToken);
        return invoices
            .Select(x => new InvoiceDto(x.Id, x.SubscriptionId, x.Amount, x.Status.ToString(), x.IssuedOnUtc))
            .ToList();
    }
}
