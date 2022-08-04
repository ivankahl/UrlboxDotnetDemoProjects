namespace ReadingListApi.Services.Screenshot
{
    public interface IWebhookScreenshotService
    {
        Task<string> SendScreenshotUrlRequestAsync(string url);

        Task<(byte[] FileBytes, string RenderId)> ProcessScreenshotUrlWebhookAsync(dynamic webhook);
    }
}