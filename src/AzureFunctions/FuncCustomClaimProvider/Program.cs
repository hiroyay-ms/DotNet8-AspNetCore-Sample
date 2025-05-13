using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using FuncCustomClaimProvider.Data;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        var connectionString = Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING") ?? throw new InvalidOperationException("Connection string 'SQL_CONNECTION_STRING' not found.");
        services.AddDbContext<AdventureWorksContext>(options => options.UseSqlServer(connectionString));
    })
    .Build();

host.Run();
