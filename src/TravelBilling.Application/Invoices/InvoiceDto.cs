namespace TravelBilling.Application.Invoices;

public sealed record InvoiceDto(Guid InvoiceId, Guid SubscriptionId, decimal Amount, string Status, DateTime IssuedOnUtc);
