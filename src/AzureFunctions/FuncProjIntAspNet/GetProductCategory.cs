using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FuncProjIntAspNet
{
    public class GetProductCategory
    {
        private readonly ILogger _logger;

        public GetProductCategory(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GetProductCategory>();
        }

        [Function("GetProductCategory")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            return new OkObjectResult("Welcome!");
        }
    }
}
