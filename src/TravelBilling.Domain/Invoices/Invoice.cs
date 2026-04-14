using TravelBilling.Domain.Common;
using TravelBilling.Domain.Events;

namespace TravelBilling.Domain.Invoices;

public sealed class Invoice : AggregateRoot
{
    private Invoice()
    {
    }

    private Invoice(Guid id, Guid customerId, Guid subscriptionId, decimal amount, DateTime issuedOnUtc)
    {
        Id = id;
        CustomerId = customerId;
        SubscriptionId = subscriptionId;
        Amount = amount;
        IssuedOnUtc = issuedOnUtc;
        Status = InvoiceStatus.Pending;
    }

    public Guid CustomerId { get; private set; }
    public Guid SubscriptionId { get; private set; }
    public decimal Amount { get; private set; }
    public DateTime IssuedOnUtc { get; private set; }
    public InvoiceStatus Status { get; private set; }

    public static Invoice Generate(Guid customerId, Guid subscriptionId, decimal amount, DateTime issuedOnUtc)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Invoice amount must be greater than zero.");
        }

        var invoice = new Invoice(Guid.NewGuid(), customerId, subscriptionId, amount, issuedOnUtc);
        invoice.AddDomainEvent(new InvoiceGenerated(invoice.Id, subscriptionId, customerId, issuedOnUtc));
        return invoice;
    }

    public void MarkAsPaid(DateTime paidAtUtc)
    {
        if (Status == InvoiceStatus.Paid)
        {
            throw new InvalidOperationException("Invoice cannot be paid twice.");
        }

        Status = InvoiceStatus.Paid;
        AddDomainEvent(new PaymentReceived(Id, CustomerId, paidAtUtc));
    }
}
