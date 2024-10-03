using System.Text.Json;
using Aspnet.Web.Models;


namespace Aspnet.Web.Services;

public class ProductService
{
    private readonly ILogger<ProductService> _logger;
    HttpClient _httpClient;

    public ProductService(HttpClient httpClient, ILogger<ProductService> logger)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task<List<ProductCategory>> GetProductCategories(string actionName)
    {
        List<ProductCategory>? productCategories = null;

        try
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"api/Product/{actionName}");

            var response = await _httpClient.SendAsync(httpRequestMessage);
            var resonseText = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                productCategories = JsonSerializer.Deserialize<List<ProductCategory>>(resonseText);

                _logger.LogInformation($"successfully retrieved product categories.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting product categories.");
        }

        return productCategories ?? new List<ProductCategory>();
    }
}