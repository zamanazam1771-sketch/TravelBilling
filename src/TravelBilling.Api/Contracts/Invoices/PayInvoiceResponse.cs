namespace TravelBilling.Api.Contracts.Invoices;

public sealed record PayInvoiceResponse(Guid InvoiceId, string Status);
