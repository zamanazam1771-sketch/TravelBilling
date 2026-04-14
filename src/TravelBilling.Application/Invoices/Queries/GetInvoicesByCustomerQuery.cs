using MediatR;

namespace TravelBilling.Application.Invoices;

public sealed record GetInvoicesByCustomerQuery(Guid CustomerId) : IRequest<IReadOnlyList<InvoiceDto>>;
