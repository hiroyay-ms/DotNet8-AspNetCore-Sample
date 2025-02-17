using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Web.Data;

namespace Web.Pages.SQL;

public class ExecuteAsModel : PageModel
{
    private readonly ILogger<ExecuteAsModel> _logger;
    private readonly AdventureWorksContext _context;

    public ExecuteAsModel(ILogger<ExecuteAsModel> logger, AdventureWorksContext context)
    {
        _logger = logger;
        _context = context;
    }

    [BindProperty(Name="schema", SupportsGet = true)]
    public string? schema { get; set; } = string.Empty;

    [BindProperty(Name="userid", SupportsGet = true)]
    public string? userid {get; set;} = string.Empty;

    public async Task OnGetAsync()
    {
        var conn = _context.Database.GetDbConnection() as SqlConnection;

        if (conn == null)
            throw new InvalidOperationException("Connection is null");
        

        ViewData["connectionString"] = conn.ConnectionString;

        if (string.IsNullOrEmpty(schema) || string.IsNullOrEmpty(userid)) {
            ViewData["queryResult"] = "Please provide the user id and schema name.";
            return;
        }

        var query = string.Empty;
        if (schema == "SalesLT")
            query = $"EXECUTE AS USER = {userid}; SELECT TOP 10 * FROM SalesLT.Product;";
        else
            query = $"EXECUTE AS USER = {userid}; SELECT TOP 10 * FROM Pubs.titles";

        ViewData["query"] = query;

        if (conn.State != System.Data.ConnectionState.Open)
            await conn.OpenAsync();
        
        try {
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new SqlParameter("@userid", userid));
                cmd.CommandText = "EXECUTE AS USER = @userid";
                cmd.ExecuteNonQuery();
            }

            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true
            };

            if (schema == "SalesLT")
            {
                var products = await _context.Product.ToListAsync();

                ViewData["queryResult"] = JsonSerializer.Serialize(products, options);
            }
            else
            {
                var titles = await _context.Titles.ToListAsync();

                ViewData["queryResult"] = JsonSerializer.Serialize(titles, options);
            }
        }
        catch (Exception ex) {
            ViewData["queryResult"] = ex.Message;
        }
        finally {
            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync();

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "REVERT";
                cmd.ExecuteNonQuery();
            }
        }
    }
}