using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using FuncProjIntAspNet.Data;

namespace FuncProjIntAspNet
{
    public class GetProductCategory
    {
        private readonly ILogger _logger;
        private readonly AdventureWorksContext _context;

        public GetProductCategory(ILoggerFactory loggerFactory, AdventureWorksContext context)
        {
            _logger = loggerFactory.CreateLogger<GetProductCategory>();
            _context = context;
        }

        [Function("GetProductCategory")]
        public async Task<IResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var query = from a in _context.ProductCategory 
                        join b in _context.ProductCategory on a.ProductCategoryID equals b.ParentProductCategoryID 
                        join p in _context.Product on b.ProductCategoryID equals p.ProductCategoryID 
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
}
