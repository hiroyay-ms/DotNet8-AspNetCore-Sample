using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Web;
using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace Web.Pages.Api;

[Authorize]
public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    private readonly IConfiguration _configuration;
    private readonly ITokenAcquisition _tokenAcquisition;
    private readonly IHttpClientFactory _httpClientFactory;

    public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration, ITokenAcquisition tokenAcquisition, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _configuration = configuration;
        _tokenAcquisition = tokenAcquisition;
        _httpClientFactory = httpClientFactory;
    }

    public async Task OnGet()
    {
        string tenantId = User.Claims.ToList().Find(c => c.Type == "tenantId")?.Value ?? throw new InvalidOperationException("Tenant ID claim not found.");
        _logger.LogInformation($"Tenant ID: {tenantId}");

        string servicePlan = User.Claims.ToList().Find(c => c.Type == "servicePlan")?.Value ?? throw new InvalidOperationException("Service plan claim not found.");
        _logger.LogInformation($"Service Plan: {servicePlan}");

        string key = _configuration.GetValue<string>($"{servicePlan}") ?? throw new InvalidOperationException($"Key '{servicePlan}' not found in configuration.");
        _logger.LogInformation($"Key: {key}");

        var httpClient = _httpClientFactory.CreateClient("BackendApi");
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"api/category?tenantId={tenantId}");
        httpRequestMessage.Headers.Add("Ocp-Apim-Subscription-Key", key);

        var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

        if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
        {
            var obj = JsonSerializer.Deserialize<object>(await httpResponseMessage.Content.ReadAsStringAsync());
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true
            };

            ViewData["apiResult"] = JsonSerializer.Serialize(obj, options);
            _logger.LogInformation($"HttpResponseStatusCode: {httpResponseMessage.ReasonPhrase}");
        }
        else
        {
            ViewData["apiResult"] = $"Error: {httpResponseMessage.ReasonPhrase}";
        }

        //string[] scopes = new []{ "user.read" };
        //string accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(scopes, user: User);

        //ViewData["AccessToken"] = accessToken;

        //_logger.LogInformation($"Access Token: {accessToken}");
    }
}