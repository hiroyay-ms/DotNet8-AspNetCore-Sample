using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Nodes;

using FuncCustomClaimProvider.Data;
using FuncCustomClaimProvider.Models;

namespace FuncCustomClaimProvider
{
    public class GetTenantAttributes
    {
        private readonly ILogger _logger;
        private readonly AdventureWorksContext _context;

        public GetTenantAttributes(ILoggerFactory loggerFactory, AdventureWorksContext context)
        {
            _logger = loggerFactory.CreateLogger<GetTenantAttributes>();
            _context = context;
        }

        [Function("GetTenantAttributes")]
        public async Task<IResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            _logger.LogInformation($"Request body: {requestBody}");

            var node = JsonNode.Parse(requestBody) ?? throw new InvalidOperationException("Invalid request body.");

            string? mail = node["data"]?["authenticationContext"]?["user"]?["mail"]?.GetValue<string>();
            _logger.LogInformation($"User mail: {mail}");

            string? domain = mail?.Substring(mail.IndexOf('@') + 1);
            _logger.LogInformation($"User domain: {domain}");

            var query = from c in _context.Customers
                        where c.EmailAddressDomain == domain
                        select c;
            
            var customer = await query.FirstOrDefaultAsync();

            ResponseContent responseContent = new ResponseContent();
            responseContent.data.actions[0].claims.tenantGuid = customer?.CustomerGuid;
            responseContent.data.actions[0].claims.servicePlan = customer?.ServicePlan;

            string jsonString = JsonSerializer.Serialize(responseContent);
            _logger.LogInformation($"Response body: {jsonString}");

            return TypedResults.Ok(responseContent);
        }
    }
}
