using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TravelBilling.Application.Subscriptions;

namespace TravelBilling.Infrastructure.Background;

public sealed class BillingCycleWorker(IServiceScopeFactory scopeFactory, ILogger<BillingCycleWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = scopeFactory.CreateScope();
                var sender = scope.ServiceProvider.GetRequiredService<ISender>();
                await sender.Send(new GenerateBillingCycleInvoicesCommand(), stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Billing cycle execution failed.");
            }

            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }
}
