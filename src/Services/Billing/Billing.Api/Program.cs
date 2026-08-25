using Billing.Api.ExceptionHandling;
using Billing.Application.Interfaces;
using Billing.Application.Services;
using Billing.Infrastructure.Clients;
using Billing.Infrastructure.Data;
using Billing.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Exception handling
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Database
builder.Services.AddDbContext<BillingDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString(
            "BillingDatabase")));

// Application
builder.Services.AddScoped<IInvoiceService, InvoiceService>();

// Repositories
builder.Services.AddScoped<
    IInvoiceRepository,
    InvoiceRepository>();

// HTTP clients
builder.Services.AddHttpClient<
    IInventoryClient,
    InventoryClient>(client =>
    {
        var baseUrl =
            builder.Configuration[
                "Services:Inventory:BaseUrl"];

        client.BaseAddress = new Uri(baseUrl!);
    });

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Global exception handling
app.UseExceptionHandler();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();