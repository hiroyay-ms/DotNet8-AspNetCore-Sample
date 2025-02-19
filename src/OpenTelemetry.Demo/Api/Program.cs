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

var connectionString = builder.Configuration["SQL_CONNECTION_STRING"] ?? throw new InvalidOperationException("Connection string 'SQL_CONNECTION_STRING' not found.");
builder.Services.AddDbContext<AdventureWorksContext>(options => options.UseSqlServer(connectionString));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
