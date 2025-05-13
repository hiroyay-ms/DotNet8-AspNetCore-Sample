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
    public class GetProductByCategoryId
    {
        private readonly ILogger _logger;
        private readonly AdventureWorksContext _context;

        public GetProductByCategoryId(ILoggerFactory loggerFactory, AdventureWorksContext context)
        {
            _logger = loggerFactory.CreateLogger<GetProductByCategoryId>();
            _context = context;
        }

        [Function("GetProductByCategoryId")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "Product/{id:int}")] HttpRequestData req, int id)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            HttpResponseData response;

            try {
                response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", "application/json");

                var query = from p in _context.Product
                    join pc in _context.ProductCategory on p.ProductCategoryID equals pc.ProductCategoryID
                    join pm in _context.ProductModel on p.ProductModelID equals pm.ProductModelID
                    join pmd in _context.ProductModelProductDescription on pm.ProductModelID equals pmd.ProductModelID
                    join pd in _context.ProductDescription on pmd.ProductDescriptionID equals pd.ProductDescriptionID
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
                
                var products = query.ToList();

                var options = new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                    WriteIndented = true
                };

                var result = JsonSerializer.Serialize(products, options);
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
