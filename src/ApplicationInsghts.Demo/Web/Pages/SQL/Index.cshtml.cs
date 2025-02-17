using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Web.Data;

namespace Web.Pages.SQL;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly AdventureWorksContext _context;

    public IndexModel(ILogger<IndexModel> logger, AdventureWorksContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task OnGet()
    {
        var conn = _context.Database.GetDbConnection() as SqlConnection;

        if (conn == null)
            throw new InvalidOperationException("Connection is null");
        

        ViewData["connectionString"] = conn.ConnectionString;
        ViewData["query"] = "SELECT TOP 10 * FROM SalesLT.Product";

        if (conn.State != System.Data.ConnectionState.Open)
            await conn.OpenAsync();
        
        var products = await _context.Product.Take(10).ToListAsync();

        var options = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            WriteIndented = true
        };

        ViewData["queryResult"] = JsonSerializer.Serialize(products, options);
    }
}