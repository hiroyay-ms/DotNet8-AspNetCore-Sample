using System.Linq;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.Json;

using Api.Data;

public static class TestEndpoints
{
    public static void RegisterTestEndpoints(this WebApplication app)
    {
        app.MapGet("/api/test/{schema}/{userid}", GetData)
            .WithName("GetData")
            .WithOpenApi();
    }

    static async Task<IResult> GetData(string schema, string userid, AdventureWorksContext db)
    {
        if (string.IsNullOrEmpty(schema) || string.IsNullOrEmpty(userid))
            return TypedResults.BadRequest();
        
        var conn = db.Database.GetDbConnection() as SqlConnection;
        if (conn == null)
                return TypedResults.Problem("Connection is null");

        string json = string.Empty;

        try {
            if (conn.State != ConnectionState.Open)
                conn.Open();

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new SqlParameter("@userid", userid));
                cmd.CommandText = "EXECUTE AS USER = @userid";
                cmd.ExecuteNonQuery();
            }

            if (schema == "SalesLT")
            {
                var products = await db.Product.ToListAsync();

                json = JsonSerializer.Serialize(products);
            }
            else
            {
                var titles = await db.Titles.ToListAsync();

                json = JsonSerializer.Serialize(titles);
            }
        } 
        catch (Exception ex) {
            json = "{\"errorMessage\": \"" + ex.Message + "\"}";
        } 
        finally {
            if (conn.State != ConnectionState.Open)
                conn.Open();

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "REVERT";
                cmd.ExecuteNonQuery();
            }
        }
        
        return TypedResults.Ok(json);
    }
}