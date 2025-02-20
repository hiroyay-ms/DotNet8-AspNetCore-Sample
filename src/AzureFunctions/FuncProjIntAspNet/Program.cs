using Azure.Monitor.OpenTelemetry.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using OpenTelemetry.Trace;
using FuncProjIntAspNet.Data;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication(builder =>
    {
        builder.UseMiddleware<HttpContextAccessorMiddleware>();
    })
    .ConfigureServices(services =>
    {
        services.AddHttpContextAccessor();
        
        services.ConfigureOpenTelemetryTracerProvider((sp, tracerProviderBuilder) => {
            tracerProviderBuilder.AddSource("AzureFunctions.FuncProjIntAspNet");
            tracerProviderBuilder.AddProcessor(new LogsWithTenantIdProcessor(new HttpContextAccessor()));
        });
        services.AddOpenTelemetry().UseAzureMonitor();
        
        //var connectionString = Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING") ?? throw new InvalidOperationException("Connection string 'SQL_CONNECTION_STRING' not found.");
        //services.AddDbContext<AdventureWorksContext>(options => options.UseSqlServer(connectionString));
        services.AddDbContext<AdventureWorksContext>();
    })
    .Build();

host.Run();
