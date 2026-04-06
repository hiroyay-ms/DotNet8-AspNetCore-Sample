using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using Azure.Developer.Playwright;
using Azure.Identity;

namespace OpenTelemetry.Demo.test;

public class ServicePageTest : PageTest
{
    public override async Task<(string, BrowserTypeConnectOptions?)?> ConnectOptionsAsync()
    {
        PlaywrightServiceBrowserClient client = new PlaywrightServiceBrowserClient(
            credential: new DefaultAzureCredential(),
            options: new PlaywrightServiceBrowserClientOptions
            {
                ServiceAuth = ServiceAuthType.EntraId
            }
        );
        var connectOptions = await client.GetConnectOptionsAsync<BrowserTypeConnectOptions>();
        return (connectOptions.WsEndpoint, connectOptions.Options);
    }
}