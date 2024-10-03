using System.Text.Json.Serialization;

namespace Aspnet.Web.Models;

public class ProductCategory
{
    [JsonPropertyName("productCategoryID")]
    public int ProductCategoryId { get; set; }

    [JsonPropertyName("category")]
    public string? Category { get; set; }

    [JsonPropertyName("subCategory")]
    public string? SubCategory { get; set; }

    [JsonPropertyName("productCount")]
    public int ProductCount { get; set; }
}