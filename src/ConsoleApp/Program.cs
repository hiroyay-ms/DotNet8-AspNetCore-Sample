using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using System.Data;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var tenantId = configuration["TenantId"];
var applicationId = configuration["ApplicationId"];
var clientKey = configuration["ClientSecret"];

var scopes = new [] { "https://database.windows.net//.default" };
var app = ConfidentialClientApplicationBuilder
    .Create(applicationId)
    .WithClientSecret(clientKey)
    .WithTenantId(tenantId)
    .Build();
var result = app.AcquireTokenForClient(scopes).ExecuteAsync().Result;

var connectionString = configuration["SqlConnectionString"];
var conn = new SqlConnection(connectionString);
conn.AccessToken = result.AccessToken;

conn.Open();

var version = string.Empty;

using (var cmd = conn.CreateCommand())
{
    cmd.CommandType = CommandType.Text;
    cmd.CommandText = "SELECT @@VERSION";

    var queryResult = cmd.ExecuteScalar();
    version = $"VERSION: {queryResult}";
}

// See https://aka.ms/new-console-template for more information
Console.WriteLine($"Hello, World! {version.ToString()}");
