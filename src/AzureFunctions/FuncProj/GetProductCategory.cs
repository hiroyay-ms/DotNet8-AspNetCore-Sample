using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using FuncProj.Data;

namespace FuncProj
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
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "ProductCategory")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            HttpResponseData response; 

            try {
                response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", "application/json");

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
                
                var productCategories = query.ToList();

                var options = new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                    WriteIndented = true
                };

                var result = JsonSerializer.Serialize(productCategories, options);
                response.WriteString(result);
            }
            catch (Exception ex) {
                _logger.LogInformation("An error occurred.");
                
                response = req.CreateResponse(HttpStatusCode.InternalServerError);
                response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
                response.WriteString(ex.Message);
            }

            return response;
        }
    }
}
