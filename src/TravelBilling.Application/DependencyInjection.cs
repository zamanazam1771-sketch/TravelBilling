using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TravelBilling.Domain.Services;

namespace TravelBilling.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        services.AddScoped<ISubscriptionBillingDomainService, SubscriptionBillingDomainService>();
        return services;
    }
}
