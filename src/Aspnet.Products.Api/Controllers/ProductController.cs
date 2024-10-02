using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Aspnet.Products.Api.Data;

namespace Aspnet.Products.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly ILogger<ProductController> _logger;
    private readonly AdventureWorksContext _context;

    public ProductController(ILogger<ProductController> logger, AdventureWorksContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    [Route("Category")]
    public async Task<IActionResult> GetProductCategories()
    {
        _logger.LogInformation("GetProductCategories processed a request.");

        var query = from a in _context.ProductCategories 
                    join b in _context.ProductCategories on a.ProductCategoryId equals b.ParentProductCategoryId 
                    join p in _context.Products on b.ProductCategoryId equals p.ProductCategoryId 
                    group new { a, b, p } by new { b.ProductCategoryId, Category = a.CategoryName, SubCategory = b.CategoryName } into g 
                    orderby g.Key.ProductCategoryId  
                    select new 
                    {
                        ProductCategoryId = g.Key.ProductCategoryId, 
                        Category = g.Key.Category,
                        SubCategory = g.Key.SubCategory,
                        ProductCount = g.Count()
                    };

        var productCategories = await query.ToListAsync();

        _logger.LogInformation($"{productCategories.Count()} product categories found.");

        return Ok(productCategories);
    }

    [HttpGet]
    [Route("Category/{category}")]
    public async Task<IActionResult> GetProductCategoriesByName(string category)
    {
        _logger.LogInformation("GetProductCategoryByName processed a request.");

        var query = from a in _context.ProductCategories 
                    join b in _context.ProductCategories on a.ProductCategoryId equals b.ParentProductCategoryId 
                    join p in _context.Products on b.ProductCategoryId equals p.ProductCategoryId 
                    where a.CategoryName == category
                    group new { a, b, p } by new { b.ProductCategoryId, Category = a.CategoryName, SubCategory = b.CategoryName } into g 
                    orderby g.Key.ProductCategoryId  
                    select new 
                    {
                        ProductCategoryId = g.Key.ProductCategoryId, 
                        Category = g.Key.Category,
                        SubCategory = g.Key.SubCategory,
                        ProductCount = g.Count()
                    };

        var productCategories = await query.ToListAsync();

        _logger.LogInformation($"{productCategories.Count()} product categories found.");

        return Ok(productCategories);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetProducts(int id)
    {
        _logger.LogInformation("Getting product with Category Id {id}", id);

        var query = from pc in _context.ProductCategories 
                    join psc in _context.ProductCategories on pc.ProductCategoryId equals psc.ParentProductCategoryId 
                    join p in _context.Products on psc.ProductCategoryId equals p.ProductCategoryId 
                    join pm in _context.ProductModels on p.ProductModelId equals pm.ProductModelId 
                    join pmd in _context.ProductModelProductDescriptions on pm.ProductModelId equals pmd.ProductModelId 
                    join pd in _context.ProductDescriptions on pmd.ProductDescriptionId equals pd.ProductDescriptionId 
                    where pmd.Culture == "en" && psc.ProductCategoryId == id  
                    select new 
                    {
                        p.ProductId,
                        p.ProductName,
                        pm.ModelName,
                        p.ProductNumber,
                        p.Color,
                        p.StandardCost,
                        p.ListPrice,
                        p.Size,
                        p.Weight,
                        p.ProductCategoryId,
                        CategoryName = pc.CategoryName,
                        SubCategoryName = psc.CategoryName,
                        pd.Description,
                        p.SellStartDate,
                        p.SellEndDate,
                        p.ThumbnailPhotoFileName
                    };

        var products = await query.ToListAsync();

        _logger.LogInformation($"Found {products.Count()} products");

        return Ok(products);
    }
}