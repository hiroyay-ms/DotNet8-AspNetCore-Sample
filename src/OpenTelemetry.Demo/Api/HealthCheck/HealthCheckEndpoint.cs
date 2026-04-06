using System.Linq;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OpenApi;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Api.Data;

public static class HealthCheckEndpoint
{
    public static void RegisterHealthCheckEndpoint(this WebApplication app)
    {
        app.MapGet("/health", GetResults)
            .WithName("HealthCheck")
            .WithOpenApi()
            .AllowAnonymous();
    }

    static async Task<IResult> GetResults(AdventureWorksContext db, [FromServices] ILoggerFactory loggerFactory)
    {
        try
        {
            var conn = db.Database.GetDbConnection() as SqlConnection;
            if (conn == null)
                return TypedResults.Problem("Connection is null");
            
            if (conn.State != ConnectionState.Open)
                conn.Open();
            
            return Results.Ok("Healthy");
        }
        catch (Exception ex)
        {
            var logger = loggerFactory.CreateLogger("HealthCheck");
            logger.LogError(ex, "Health check failed");

            return Results.Problem("Health check failed");
        }
    }
}