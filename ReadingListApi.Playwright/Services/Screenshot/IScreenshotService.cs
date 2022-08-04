namespace ReadingListApi.Services.Screenshot
{
    public interface IScreenshotService
    {
        Task<byte[]> ScreenshotUrlAsync(string url); 
    }
}