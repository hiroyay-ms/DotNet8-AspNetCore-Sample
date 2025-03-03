using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Nodes;

using FuncCustomClaimProvider.Models;

namespace FuncCustomClaimProvider
{
    public class GetTenantAttributes
    {
        private readonly ILogger _logger;

        public GetTenantAttributes(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GetTenantAttributes>();
        }

        [Function("GetTenantAttributes")]
        public async Task<IResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            _logger.LogInformation($"Request body: {requestBody}");

            dynamic data = JsonNode.Parse(requestBody);

            //string userPrincipalName = data?.data.authenticationContext.user.userPrincipalName;
            //string domain = userPrincipalName?.Substring(userPrincipalName.IndexOf('@') + 1);

            //_logger.LogInformation($"User principal name: {userPrincipalName}");
            //_logger.LogInformation($"Domain: {domain}");

            ResponseContent responseContent = new ResponseContent();
            responseContent.data.actions[0].claims.TenantId = Guid.NewGuid().ToString();

            return TypedResults.Ok(responseContent);
        }
    }
}
