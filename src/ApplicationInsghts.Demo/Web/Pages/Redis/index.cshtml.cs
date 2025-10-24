using Azure.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Web.Data;

namespace Web.Pages.Redis;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;

    public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
    }

    [BindProperty(SupportsGet = true)]
    public string? ProductId { get; set; }

    [BindProperty]
    public string? ElapsedTime { get; set; }

    public async Task OnGet()
    {
        if (string.IsNullOrEmpty(ProductId))
        {
            ViewData["queryResult"] = "Please provide the product ID.";
            return;
        }

        string? json;
        var watch = Stopwatch.StartNew();

        var keyName = $"product:{ProductId}";

        var redisHostName = _configuration.GetValue<string>("RedisHostName") ?? throw new InvalidOperationException("Environment variable 'RedisHostName' not found.");

        var configurationOptions = await ConfigurationOptions.Parse($"{redisHostName}").ConfigureForAzureWithTokenCredentialAsync(new DefaultAzureCredential());
        ConnectionMultiplexer redisConnection = await ConnectionMultiplexer.ConnectAsync(configurationOptions);
        IDatabase db = redisConnection.GetDatabase();

        json = await db.StringGetAsync(keyName);

        if (string.IsNullOrEmpty(json))
        {
            var baseUrl = _configuration.GetValue<string>("API_BASE_ADDRESS") ?? throw new InvalidOperationException("Environment variable 'API_BASE_ADDRESS' not found.");

            var httpClient = _httpClientFactory.CreateClient();
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"{baseUrl}api/category/{ProductId}/products");

            var response = await httpClient.SendAsync(httpRequestMessage);

            if (response.IsSuccessStatusCode)
            {
                var obj = JsonSerializer.Deserialize<object>(await response.Content.ReadAsStringAsync());
                var options = new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                    WriteIndented = true
                };

                json = JsonSerializer.Serialize(obj, options);
                var setTask = db.StringSetAsync(keyName, json);
                var expireTask = db.KeyExpireAsync(keyName, TimeSpan.FromSeconds(3600));

                await Task.WhenAll(setTask, expireTask);
            }
            else
            {
                json = $"Error: {response.ReasonPhrase}";
            }
        }

        watch.Stop();

        ElapsedTime = watch.ToString();
        ViewData["queryResult"] = json;
    }
}