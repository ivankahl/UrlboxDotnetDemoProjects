using System.Net.Http.Headers;
using System.Text.Json.Nodes;

namespace ReadingListApi.Services.Screenshot
{
    public class UrlboxWebhookScreenshotService : IWebhookScreenshotService
    {
        private readonly IConfiguration _configuration;

        public UrlboxWebhookScreenshotService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<(byte[] FileBytes, string RenderId)> ProcessScreenshotUrlWebhookAsync(dynamic webhook)
        {
            // Retrieve the Render URL from the dynamic object and convert it to a string
            var renderUrl = webhook.result.renderUrl?.ToString();

            // Always a good idea to implement error handling in case we can't find the
            // renderURL
            if (renderUrl == null)
                throw new Exception("renderUrl is null!");

            // Use an HTTP client to query the renderUrl. If successful, the raw bytes
            // of the screenshot are returned along with the renderId field on the webhook.
            // Otherwise, throw an exception with the response body in the message.
            using (var client = new HttpClient())
            {
                using (var result = await client.GetAsync(renderUrl))
                {
                    if (result.IsSuccessStatusCode)
                    {
                        return (await result.Content.ReadAsByteArrayAsync(), webhook.renderId);
                    }
                    else
                    {
                        throw new Exception($"Failed to download renderUrl: {await result.Content.ReadAsStringAsync()}");
                    }
                }
            }
        }

        public async Task<string> SendScreenshotUrlRequestAsync(string url)
        {
            // Create a dictionary of options for the screenshot. This is
            // the same as the previous dictionary with the addition of one
            // configuration option: webhook_url.
            var options = new Dictionary<string, object> {
                ["format"] = "png",
                ["url"] = url,
                ["width"] = 1920,
                ["height"] = 1080,
                ["block_ads"] = true,
                ["hide_cookie_banners"] = true,
                ["retina"] = true,
                ["click_accept"] = true,
                ["hide_selector"] = ".pn-ribbon",
                // Specify the endpoint that that webhook can be sent to
                ["webhook_url"] = _configuration["BaseUrl"] + "/ReadingList/webhooks/screenshot"
            };

            // Send the request to the https://api.urlbox.io/v1/render endpoint
            using (var client = new HttpClient())
            {
                // For authentication, you must use "Bearer <ApiSecret>" in the Authorization header
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _configuration["Urlbox:ApiSecret"]);

                using (var result = await client.PostAsJsonAsync("https://api.urlbox.io/v1/render", options))
                {
                    if (result.IsSuccessStatusCode)
                    {
                        // Retrieve the JSON response as a string and parse it. This is so you can
                        // get the renderId property which you can store to later reconcile the
                        // webhook with the correct reading item.
                        var json = JsonObject.Parse(await result.Content.ReadAsStringAsync());
                        var renderId = json?["renderId"]?.GetValue<string>();

                        if (renderId == null)
                            throw new Exception("No renderId returned!");

                        return renderId;
                    }
                    else
                    {
                        throw new Exception($"Failed to send render request: {await result.Content.ReadAsStringAsync()}");
                    }
                }
            }
        }
    }
}