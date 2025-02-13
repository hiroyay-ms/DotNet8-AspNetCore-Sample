using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Web.Data;

namespace Web.Pages.Api;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
     private readonly IHttpClientFactory _httpClientFactory;

    public IndexModel(ILogger<IndexModel> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    [BindProperty(Name="tenant", SupportsGet = true)]
    public string? tenant { get; set; } = string.Empty;

    public async Task OnGet()
    {
        if (string.IsNullOrEmpty(tenant)) {
            ViewData["queryResult"] = "Please provide the tenant name.";
            return;
        }

        var httpClient = _httpClientFactory.CreateClient("api");
        httpClient.DefaultRequestHeaders.Add("x-tenant-id", tenant);

        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"api/category");

        var response = await httpClient.SendAsync(httpRequestMessage);

        if (response.IsSuccessStatusCode)
        {
            ViewData["queryResult"] = await response.Content.ReadAsStringAsync();
        }
        else
        {
            ViewData["queryResult"] = $"Error: {response.StatusCode}";
        }
    }
}