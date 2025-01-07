using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using Azure.Identity;
using Azure.Storage.Blobs;

namespace Web.Pages.Blob;

public class ManagedIdentityModel : PageModel
{
    private readonly ILogger<ManagedIdentityModel> _logger;
    private readonly IConfiguration _configuration;

    public ManagedIdentityModel(ILogger<ManagedIdentityModel> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    [BindProperty(Name="container", SupportsGet = true)]
    public string? container { get; set; } = string.Empty;

    [BindProperty(Name="userid", SupportsGet = true)]
    public string? userid { get; set; } = string.Empty;

    [BindProperty(Name="blob", SupportsGet = true)]
    public string? blob { get; set; } = string.Empty;

    public void OnGet()
    {
        var storageAccountName =  _configuration.GetValue<string>("STORAGE_ACCOUNT_NAME") ?? throw new InvalidOperationException("Environment variable 'STORAGE_ACCOUNT_NAME' not found.");
        var blobService = $"https://{storageAccountName}.blob.core.windows.net";

        ViewData["blobService"] = blobService;

        if (string.IsNullOrEmpty(container)) {
            ViewData["message"] = "Please provide the container name.";
            return;
        }

        try {
            DefaultAzureCredential credential = new DefaultAzureCredential(new DefaultAzureCredentialOptions { ManagedIdentityClientId = userid });
            BlobContainerClient containerClient = new BlobContainerClient(new Uri($"{blobService}/{container}"), credential);

            var fileName = $"{Guid.NewGuid()}.txt";

            BlobClient blobClient = containerClient.GetBlobClient(fileName);
            
            string text = $"Hello World from {userid} -{DateTime.Now}";
            blobClient.Upload(BinaryData.FromString(text), overwrite: true);

            blob = $"{blobService}/{container}/{fileName}";
            ViewData["message"] = $"The file was successfully uploaded on blob storage.";
        }
        catch (Exception ex) {
            ViewData["message"] = $"The file was not successfully uploaded on blob storage. {ex.Message}";
        }
    }
}