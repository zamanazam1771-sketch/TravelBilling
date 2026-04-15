using TravelBilling.Domain.Customers;
using TravelBilling.Domain.Invoices;
using TravelBilling.Domain.Subscriptions;

namespace TravelBilling.Domain.Tests;

public class SubscriptionDomainTests
{
    [Fact]
    public void Activate_ShouldSetSubscriptionActiveAndScheduleNextBilling()
    {
        var subscription = Subscription.Create(Guid.NewGuid(), "Traveler Pro", 49.99m, 30);
        var now = DateTime.UtcNow;

        subscription.Activate(now);

        Assert.Equal(SubscriptionStatus.Active, subscription.Status);
        Assert.Equal(now.AddDays(30), subscription.NextBillingDateUtc);
    }

    [Fact]
    public void CancelledSubscription_ShouldNotGenerateFutureInvoices()
    {
        var subscription = Subscription.Create(Guid.NewGuid(), "Traveler Pro", 49.99m, 30);
        var now = DateTime.UtcNow;
        subscription.Activate(now);
        subscription.Cancel(now.AddDays(1));

        var action = () => subscription.AdvanceBillingCycle(now.AddDays(31));

        Assert.Throws<InvalidOperationException>(action);
    }

    [Fact]
    public void PayingInvoiceTwice_ShouldThrow()
    {
        var invoice = Invoice.Generate(Guid.NewGuid(), Guid.NewGuid(), 20m, DateTime.UtcNow);
        invoice.MarkAsPaid(DateTime.UtcNow);

        var action = () => invoice.MarkAsPaid(DateTime.UtcNow);

        Assert.Throws<InvalidOperationException>(action);
    }

    [Fact]
    public void CreateCustomer_WithInvalidEmail_ShouldThrowArgumentException()
    {
        var action = () => Customer.Create("Zaman", "not-an-email");

        Assert.Throws<ArgumentException>(action);
    }

    [Fact]
    public void CreateSubscription_WithInvalidRecurringAmount_ShouldThrowArgumentException()
    {
        var action = () => Subscription.Create(Guid.NewGuid(), "Traveler Pro", 0m, 30);

        Assert.Throws<ArgumentException>(action);
    }

    [Fact]
    public void CreateSubscription_WithInvalidBillingCycle_ShouldThrowArgumentException()
    {
        var action = () => Subscription.Create(Guid.NewGuid(), "Traveler Pro", 10m, 0);

        Assert.Throws<ArgumentException>(action);
    }

    [Fact]
    public void Cancel_WhenSubscriptionIsNotActive_ShouldThrowInvalidOperationException()
    {
        var subscription = Subscription.Create(Guid.NewGuid(), "Traveler Pro", 49.99m, 30);
        var action = () => subscription.Cancel(DateTime.UtcNow);

        Assert.Throws<InvalidOperationException>(action);
    }
}
