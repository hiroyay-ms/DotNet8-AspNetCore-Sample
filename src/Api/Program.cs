using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ITelemetryInitializer, AppInsightsTelemetryInitializer>();

var connectionString = builder.Configuration["SQL_CONNECTION_STRING"] ?? throw new InvalidOperationException("Connection string 'SQL_CONNECTION_STRING' not found.");
builder.Services.AddDbContext<AdventureWorksContext>(options => options.UseSqlServer(connectionString));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
