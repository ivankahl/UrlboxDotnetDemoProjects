using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using System.Drawing;

namespace ReadingListApi.Services.Screenshot
{
    public class SeleniumScreenshotService : IScreenshotService
    {
        public Task<byte[]> ScreenshotUrlAsync(string url)
        {
            // We need to download the correct driver for Chrome
            new DriverManager().SetUpDriver(new ChromeConfig());

            var options = new ChromeOptions();
            options.AddArgument("headless");

            // Use the driver to start a new instance of Google
            // Chrome
            var driver = new ChromeDriver(options);

            // Set the window size appropriately
            driver.Manage().Window.Size = new Size(1920, 1080);

            // Navigate to the specified URL
            driver.Navigate().GoToUrl(url);

            // Take a screenshot of the web page and return the 
            // image's raw bytes
            var screenshot = (driver as ITakesScreenshot).GetScreenshot();
            var bytes = screenshot.AsByteArray;

            driver.Close();
            driver.Quit();

            return Task.FromResult(bytes);
        }
    }
}