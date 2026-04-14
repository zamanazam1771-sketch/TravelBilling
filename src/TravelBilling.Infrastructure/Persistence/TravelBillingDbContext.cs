using Microsoft.EntityFrameworkCore;
using TravelBilling.Domain.Customers;
using TravelBilling.Domain.Invoices;
using TravelBilling.Domain.Subscriptions;

namespace TravelBilling.Infrastructure.Persistence;

public sealed class TravelBillingDbContext(DbContextOptions<TravelBillingDbContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Subscription> Subscriptions => Set<Subscription>();
    public DbSet<Invoice> Invoices => Set<Invoice>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>().HasKey(x => x.Id);
        modelBuilder.Entity<Subscription>().HasKey(x => x.Id);
        modelBuilder.Entity<Invoice>().HasKey(x => x.Id);

        modelBuilder.Entity<Invoice>()
            .Property(x => x.Amount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Customer>().Ignore(x => x.DomainEvents);
        modelBuilder.Entity<Subscription>().Ignore(x => x.DomainEvents);
        modelBuilder.Entity<Invoice>().Ignore(x => x.DomainEvents);
    }
}
