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
        // string[] scopes = new []{ "api://5ec994ee-6531-434e-a531-223a88268996/access_as_user" };
        var application_id_uri = _configuration.GetValue<string>("APPLICATION_ID_URI") ?? throw new InvalidOperationException("Application ID URI 'APPLICATION_ID_URI' not found.");
        string[] scopes = new []{ $"{application_id_uri}/Data.Read.All" };

        _logger.LogInformation($"Scopes: {application_id_uri}");

        string accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(scopes);

        ViewData["AccessToken"] = accessToken;

        _logger.LogInformation($"Access Token: {accessToken}");

        string servicePlan = User.Claims.ToList().Find(c => c.Type == "servicePlan")?.Value ?? throw new InvalidOperationException("Service plan claim not found.");
        _logger.LogInformation($"Service Plan: {servicePlan}");

        string key = _configuration.GetValue<string>($"{servicePlan}") ?? throw new InvalidOperationException($"Key '{servicePlan}' not found in configuration.");

        var httpClient = _httpClientFactory.CreateClient("BackendApi");
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
        httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", key);

        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"api/category");

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
    }
}