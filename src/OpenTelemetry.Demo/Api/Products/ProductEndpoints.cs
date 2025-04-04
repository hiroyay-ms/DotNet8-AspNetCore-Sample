using System.Linq;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;

using Api.Data;

public static class ProductEndpoints
{
    public static void RegisterProductEndpoints(this WebApplication app)
    {
        app.MapGet("/api/category", GetProductCategories)
            .WithName("GetProductCategories")
            .WithOpenApi()
            .RequireAuthorization();
        
        app.MapGet("/api/category/{category}", GetProductCategoryByName)
            .WithName("GetProductCategoryByName")
            .WithOpenApi();
        
        app.MapGet("/api/category/{id}/products", GetProductsByCategoryId)
            .WithName("GetProductsByCategoryId")
            .WithOpenApi();
    }

    static async Task<IResult> GetProductCategories(AdventureWorksContext db, [FromServices] ILogger logger)
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

        logger.LogInformation("Product categories count: {Count}", productCategories.Count);
        
        return productCategories.Count == 0 ? TypedResults.NotFound() : TypedResults.Ok(productCategories);
    }

    static async Task<IResult> GetProductCategoryByName(string category, AdventureWorksContext db, [FromServices] ILogger logger)
    {
        if (string.IsNullOrEmpty(category))
            return TypedResults.BadRequest();
        
        var query = from a in db.ProductCategory 
                    join b in db.ProductCategory on a.ProductCategoryID equals b.ParentProductCategoryID 
                    join p in db.Product on b.ProductCategoryID equals p.ProductCategoryID 
                    where a.Name == category
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

        logger.LogInformation("Product categories count: {Count}", productCategories.Count);

        return TypedResults.Ok(productCategories);
    }

    static async Task<IResult> GetProductsByCategoryId(int id, AdventureWorksContext db, [FromServices] ILogger logger)
    {
        if (id < 5)
             return TypedResults.BadRequest();

        var query = from p in db.Product
                    join pc in db.ProductCategory on p.ProductCategoryID equals pc.ProductCategoryID
                    join pm in db.ProductModel on p.ProductModelID equals pm.ProductModelID
                    join pmd in db.ProductModelProductDescription on pm.ProductModelID equals pmd.ProductModelID
                    join pd in db.ProductDescription on pmd.ProductDescriptionID equals pd.ProductDescriptionID
                    where pmd.Culture == "en" && p.ProductCategoryID == id
                    select new
                    {
                        p.ProductID,
                        ProductName = p.Name,
                        p.ProductNumber,
                        p.Color,
                        p.StandardCost,
                        p.ListPrice,
                        p.Size,
                        p.Weight,
                        p.ProductCategoryID,
                        CategoryName = pc.Name,
                        p.ProductModelID,
                        ModelName = pm.Name,
                        Description = pd.Description,
                        p.SellStartDate,
                        p.SellEndDate,
                        p.ThumbnailPhotoFileName
                    };
        
        var products = await query.ToListAsync();

        logger.LogInformation("Products count: {Count}", products.Count);

        return products.Count == 0 ? TypedResults.NotFound() : TypedResults.Ok(products);
    }
}