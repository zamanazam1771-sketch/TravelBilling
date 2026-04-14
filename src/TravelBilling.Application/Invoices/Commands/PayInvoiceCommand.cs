using MediatR;

namespace TravelBilling.Application.Invoices;

public sealed record PayInvoiceCommand(Guid InvoiceId) : IRequest<bool>;
