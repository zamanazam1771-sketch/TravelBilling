using TravelBilling.Domain.Common;
using TravelBilling.Domain.ValueObjects;

namespace TravelBilling.Domain.Customers;

public sealed class Customer : AggregateRoot
{
    private Customer()
    {
    }

    private Customer(Guid id, string name, Email email)
    {
        Id = id;
        Name = name;
        Email = email.Value;
    }

    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;

    public static Customer Create(string name, string email)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Customer name is required.");
        }

        var parsedEmail = ValueObjects.Email.Create(email);
        return new Customer(Guid.NewGuid(), name.Trim(), parsedEmail);
    }
}
