using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TravelBilling.Application.Abstractions;
using TravelBilling.Application.Abstractions.Repositories;
using TravelBilling.Infrastructure.Persistence;
using TravelBilling.Infrastructure.Persistence.Repositories;
using TravelBilling.Infrastructure.Time;

namespace TravelBilling.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<TravelBillingDbContext>(options => options.UseInMemoryDatabase("travel-billing-db"));
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
        services.AddScoped<IInvoiceRepository, InvoiceRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddSingleton<IClock, SystemClock>();
        return services;
    }
}
