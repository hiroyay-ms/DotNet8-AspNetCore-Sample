using Azure.Monitor.OpenTelemetry.AspNetCore;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Trace;
using Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.ConfigureOpenTelemetryTracerProvider((sp, tracerProviderBuilder) => {
    tracerProviderBuilder.AddSource("OpenTelemetry.Demo.Api");
    tracerProviderBuilder.AddProcessor(new LogsWithTenantIdProcessor(new HttpContextAccessor()));
});
builder.Services.AddOpenTelemetry().UseAzureMonitor();

//builder.Services.AddDbContext<AdventureWorksContext>();

builder.Services.AddDbContext<AdventureWorksContext>((serviceProvider, options) =>
{
    var connectionString = builder.Configuration["SQL_CONNECTION_STRING"] ?? throw new InvalidOperationException("Connection string 'SQL_CONNECTION_STRING' not found.");

    var httpContext = serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
    var httpRequest = httpContext.Request;
    var serverName = httpRequest.Headers["x-server-name"];

    if (!string.IsNullOrEmpty(serverName))
    {
        connectionString = $"Server=tcp:{serverName}.database.windows.net,1433;Initial Catalog=AdventureWorksLT;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication=Active Directory Default;";
    }

    options.UseSqlServer(connectionString);
});

var app = builder.Build();

app.RegisterProductEndpoints();

app.Run();
