using System.Linq;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;

using Api.Data;
using Api.Models;

public static class ProductEndpoints
{
    public static void RegisterProductEndpoints(this WebApplication app)
    {
        app.MapGet("/api/categories", GetProductCategories)
            .WithName("GetProductCategories")
            .WithOpenApi();
    }

    static async Task<IResult> GetProductCategories(AdventureWorksContext db)
    {
        var query = from a in db.ProductCategory 
                    join b in db.ProductCategory on a.ProductCategoryID equals b.ParentProductCategoryID 
                    join p in db.Product on b.ProductCategoryID equals p.ProductCategoryID 
                    group new { a, b, p } by new { b.ProductCategoryID, Category = a.Name, SubCategory = b.Name } into g 
                    orderby g.Key.ProductCategoryID  
                    select new 
                    {
                        ProductCategoryID = g.Key.ProductCategoryID, 
                        Category = g.Key.Category,
                        SubCategory = g.Key.SubCategory,
                        ProductCount = g.Count()
                    };

        var productCategories = await query.ToListAsync();
        
        return productCategories.Count == 0 ? TypedResults.NotFound() : TypedResults.Ok(productCategories);
    }
}