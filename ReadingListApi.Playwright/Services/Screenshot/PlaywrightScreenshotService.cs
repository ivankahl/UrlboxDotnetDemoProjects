using Microsoft.Playwright;

namespace ReadingListApi.Services.Screenshot
{
    public class PlaywrightScreenshotService : IScreenshotService
    {
        public async Task<byte[]> ScreenshotUrlAsync(string url)
        {
            // Create a new instance of Playwright
            using var playwright = await Playwright.CreateAsync();

            // Open a new instance of the Google Chrome browser in headless mode
            await using var browser = await playwright.Chromium.LaunchAsync(new() { Headless = true });

            // Create a new page in the browser
            var page = await browser.NewPageAsync(new() 
            { 
                ViewportSize = new() { Width = 1920, Height = 1080 } 
            });
            await page.GotoAsync(url);

            // Screenshot the page and return the raw bytes
            return await page.ScreenshotAsync();
        }
    }
}