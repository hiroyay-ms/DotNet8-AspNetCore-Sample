using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OpenTelemetry.Demo.test;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class Tests : ServicePageTest
{
    [Test]
    public async Task HomepageHasPlaywrightInTitleAndGetStartedLinkLinkingtoTheIntroPage()
    {
        await Page.GotoAsync("https://playwright.dev");

        await Expect(Page).ToHaveTitleAsync(new Regex("Playwright"));

        var getStarted = Page.GetByRole(AriaRole.Link, new() { Name = "Get started" });

        await Expect(getStarted).ToHaveAttributeAsync("href", "/docs/intro");

        await getStarted.ClickAsync();

        await Expect(Page).ToHaveURLAsync(new Regex(".*intro"));
    }
}
