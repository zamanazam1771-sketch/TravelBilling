using MediatR;
using Microsoft.OpenApi;
using TravelBilling.Api.Contracts.Billing;
using TravelBilling.Api.Contracts.Customers;
using TravelBilling.Api.Contracts.Invoices;
using TravelBilling.Api.Contracts.Subscriptions;
using TravelBilling.Api.Extensions;
using TravelBilling.Application;
using TravelBilling.Application.Customers;
using TravelBilling.Application.Invoices;
using TravelBilling.Application.Subscriptions;
using TravelBilling.Infrastructure;
using TravelBilling.Infrastructure.Background;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Travel Billing API",
        Version = "v1",
        Description = "Customers, subscriptions, invoices, and billing cycle operations."
    });
});

builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.AddHostedService<BillingCycleWorker>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Travel Billing API v1");
        options.DocumentTitle = "Travel Billing API";
    });
}

app.MapPost("/customers", async (CreateCustomerRequest request, IMediator mediator, CancellationToken ct) =>
{
    var result = await mediator.Send(new CreateCustomerCommand(request.Name, request.Email), ct);
    return result.ToHttpResult(id => Results.Created($"/customers/{id}", new { CustomerId = id }));
})
.WithTags("Customers");

app.MapPost("/subscriptions", async (CreateSubscriptionRequest request, IMediator mediator, CancellationToken ct) =>
{
    var result = await mediator.Send(
        new CreateSubscriptionCommand(request.CustomerId, request.PlanName, request.RecurringAmount, request.BillingCycleDays), ct);
    return result.ToHttpResult(id => Results.Created($"/subscriptions/{id}", new { SubscriptionId = id }));
})
.WithTags("Subscriptions");

app.MapPost("/subscriptions/{subscriptionId:guid}/cancel", async (Guid subscriptionId, IMediator mediator, CancellationToken ct) =>
{
    var result = await mediator.Send(new CancelSubscriptionCommand(subscriptionId), ct);
    return result.IsSuccess
        ? Results.Ok(new CancelSubscriptionResponse(subscriptionId, "Cancelled"))
        : result.ToHttpResult();
})
.WithTags("Subscriptions");

app.MapPost("/invoices/{invoiceId:guid}/pay", async (Guid invoiceId, IMediator mediator, CancellationToken ct) =>
{
    var result = await mediator.Send(new PayInvoiceCommand(invoiceId), ct);
    return result.IsSuccess
        ? Results.Ok(new PayInvoiceResponse(invoiceId, "Paid"))
        : result.ToHttpResult();
})
.WithTags("Invoices");

app.MapGet("/customers/{customerId:guid}/invoices", async (Guid customerId, IMediator mediator, CancellationToken ct) =>
{
    var invoices = await mediator.Send(new GetInvoicesByCustomerQuery(customerId), ct);
    return Results.Ok(invoices);
})
.WithTags("Invoices");

app.MapPost("/billing/run-cycle", async (IMediator mediator, CancellationToken ct) =>
{
    var createdCount = await mediator.Send(new GenerateBillingCycleInvoicesCommand(), ct);
    return Results.Ok(new RunBillingCycleResponse(createdCount));
})
.WithTags("Billing");

app.Run();
