using MediatR;
using TravelBilling.Application.Abstractions;
using TravelBilling.Application.Abstractions.Repositories;
using TravelBilling.Application.Common;

namespace TravelBilling.Application.Invoices;

public sealed class PayInvoiceCommandHandler(IInvoiceRepository invoiceRepository, IUnitOfWork unitOfWork, IClock clock)
    : IRequestHandler<PayInvoiceCommand, CommandResult>
{
    public async Task<CommandResult> Handle(PayInvoiceCommand command, CancellationToken cancellationToken)
    {
        var invoice = await invoiceRepository.GetAsync(command.InvoiceId, cancellationToken);
        if (invoice is null)
        {
            return CommandResult.Failure(ApplicationError.NotFound("Invoice was not found."));
        }

        try
        {
            invoice.MarkAsPaid(clock.UtcNow);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return CommandResult.Success();
        }
        catch (InvalidOperationException exception)
        {
            return CommandResult.Failure(ApplicationError.Conflict(exception.Message));
        }
    }
}
