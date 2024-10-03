using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Aspnet.Web.Models;
using Aspnet.Web.Services;

namespace Aspnet.Web.Pages.Product;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly ProductService _productService;

    public IndexModel(ILogger<IndexModel> logger, ProductService productService)
    {
        _logger = logger;
        _productService = productService;
    }

    public List<ProductCategory> ProductCategories { get; set; } = default!;

    [BindProperty(SupportsGet = true)]
    public string? Category { get; set; }

    public async Task OnGetAsync()
    {
        string actionName = string.IsNullOrWhiteSpace(Category) ? "Category" : $"Category/{Category}";

        _logger.LogInformation($"Calling ProductService with actionName: {actionName}");
        ProductCategories = await _productService.GetProductCategories(actionName);

        _logger.LogInformation($"{ProductCategories.Count} product categories found.");
    }
}