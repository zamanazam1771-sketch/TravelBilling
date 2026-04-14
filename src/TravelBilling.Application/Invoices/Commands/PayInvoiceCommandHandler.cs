using MediatR;
using TravelBilling.Application.Abstractions;
using TravelBilling.Application.Abstractions.Repositories;

namespace TravelBilling.Application.Invoices;

public sealed class PayInvoiceCommandHandler(IInvoiceRepository invoiceRepository, IUnitOfWork unitOfWork, IClock clock)
    : IRequestHandler<PayInvoiceCommand, bool>
{
    public async Task<bool> Handle(PayInvoiceCommand command, CancellationToken cancellationToken)
    {
        var invoice = await invoiceRepository.GetAsync(command.InvoiceId, cancellationToken);
        if (invoice is null)
        {
            return false;
        }

        invoice.MarkAsPaid(clock.UtcNow);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
