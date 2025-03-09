using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages;

[AllowAnonymous]
public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            _logger.LogInformation("User is authenticated");
        }
        else
        {
            _logger.LogInformation("User is not authenticated");

        }
    }
}
