using System.Linq;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

using Api.Data;

public static class PubsEndpoints
{
    public static void RegisterPubsEndpoints(this WebApplication app)
    {
        app.MapGet("/api/titles", GetTitles)
            .WithName("GetTitles")
            .WithOpenApi();
        
        app.MapGet("/api/{salesrep}/publishers", GetPublishers)
            .WithName("GetPublishers")
            .WithOpenApi();
        
        app.MapGet("/api/{salesrep}/publisherbooklist", GetPublisherBookList)
            .WithName("GetPublisherBookList")
            .WithOpenApi();
    }

    static async Task<IResult> GetTitles(AdventureWorksContext db)
    {
        var titles = await db.Titles.ToListAsync();
        
        return titles.Count == 0 ? TypedResults.NotFound() : TypedResults.Ok(titles);
    }

    static async Task<IResult> GetPublishers(string salesrep, AdventureWorksContext db)
    {
        if (string.IsNullOrEmpty(salesrep))
            return TypedResults.BadRequest();
        
        var conn = db.Database.GetDbConnection() as SqlConnection;

        if (conn == null)
            throw new InvalidOperationException("Connection is null");
        
        if (conn.State != System.Data.ConnectionState.Open)
            await conn.OpenAsync();

        try
        {
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new SqlParameter("@userid", salesrep));
                cmd.CommandText = "EXECUTE AS USER = @userid";
                cmd.ExecuteNonQuery();
            }

            var publishers = await db.Publishers.ToListAsync();

            return publishers.Count == 0 ? TypedResults.NotFound() : TypedResults.Ok(publishers);
        }
        catch (Exception ex)
        {
            return TypedResults.Problem(ex.Message);
        }
        finally
        {
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "REVERT";
                cmd.ExecuteNonQuery();
            }
        }
    }

    static async Task<IResult> GetPublisherBookList(string salesrep, AdventureWorksContext db)
    {
        if (string.IsNullOrEmpty(salesrep))
            return TypedResults.BadRequest();
        
        var conn = db.Database.GetDbConnection() as SqlConnection;

        if (conn == null)
            throw new InvalidOperationException("Connection is null");
        
        if (conn.State != System.Data.ConnectionState.Open)
            await conn.OpenAsync();

        try
        {
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new SqlParameter("@userid", salesrep));
                cmd.CommandText = "EXECUTE AS USER = @userid";
                cmd.ExecuteNonQuery();
            }

            var query = from t in db.Titles 
                        join p in db.Publishers on t.pub_id equals p.pub_id 
                        select new 
                        {
                            TitleId = t.title_id,
                            BookTitle = t.title,
                            BookType = t.type,
                            Price = t.price,
                            Notes = t.notes,
                            PublisherId = t.pub_id,
                            PublisherName = p.pub_name,
                            Country = p.country
                        };
            
            var publisherBookList = await query.ToListAsync();

            return publisherBookList.Count == 0 ? TypedResults.NotFound() : TypedResults.Ok(publisherBookList);
        }
        catch (Exception ex)
        {
            return TypedResults.Problem(ex.Message);
        }
        finally
        {
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "REVERT";
                cmd.ExecuteNonQuery();
            }
        }

    }
}