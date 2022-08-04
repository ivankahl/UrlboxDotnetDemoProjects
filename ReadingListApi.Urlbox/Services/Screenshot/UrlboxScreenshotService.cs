using Screenshots;

namespace ReadingListApi.Services.Screenshot
{
    public class UrlboxScreenshotService : IScreenshotService
    {
        private readonly Urlbox _urlbox;

        public UrlboxScreenshotService(Urlbox urlbox)
        {
            _urlbox = urlbox;
        }

        public async Task<byte[]> ScreenshotUrlAsync(string url)
        {
            // Download the image
            var image = await _urlbox.DownloadAsBase64(new Dictionary<string, object> {
                ["format"] = "png",
                ["url"] = url,
                ["width"] = 1920,
                ["height"] = 1080,
                ["block_ads"] = true,
                ["hide_cookie_banners"] = true,
                ["retina"] = true,
                ["click_accept"] = true,
                ["hide_selector"] = ".pn-ribbon"
            });

            // The default Base64 string includes the content type
            // which .NET doesn't want
            // e.g. image/png;base64,XXXXXXXXXXXXXXX
            image = image.Substring(image.IndexOf(",") + 1);

            return Convert.FromBase64String(image);
        }
    }
}