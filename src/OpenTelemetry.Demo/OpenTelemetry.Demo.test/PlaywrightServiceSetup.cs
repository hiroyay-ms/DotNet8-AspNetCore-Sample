using Azure.Developer.Playwright;
using Azure.Developer.Playwright.NUnit;
using NUnit.Framework;
using Azure.Identity;

namespace OpenTelemetry.Demo.test;

[SetUpFixture]
public class PlaywrightServiceNUnitSetup : PlaywrightServiceBrowserNUnit
{
    public PlaywrightServiceNUnitSetup() : base(
        credential: new DefaultAzureCredential(),
        options: new PlaywrightServiceBrowserClientOptions
        {
            ServiceAuth = ServiceAuthType.EntraId
        }
    )
    {}
}