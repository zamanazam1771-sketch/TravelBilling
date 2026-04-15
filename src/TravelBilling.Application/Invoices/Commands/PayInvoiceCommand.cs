using MediatR;
using TravelBilling.Application.Common;

namespace TravelBilling.Application.Invoices;

public sealed record PayInvoiceCommand(Guid InvoiceId) : IRequest<CommandResult>;
