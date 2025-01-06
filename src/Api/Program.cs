using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ITelemetryInitializer, AppInsightsTelemetryInitializer>();

var connectionString = builder.Configuration["SQL_CONNECTION_STRING"] ?? throw new InvalidOperationException("Connection string 'SQL_CONNECTION_STRING' not found.");
builder.Services.AddDbContext<AdventureWorksContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "AdventureWorks API",
        Version = "v1"
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.RegisterProductEndpoints();
app.RegisterTestEndpoints();

app.Run();
