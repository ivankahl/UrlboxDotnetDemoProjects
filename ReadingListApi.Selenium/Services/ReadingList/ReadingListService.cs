using ReadingListApi.Services.ReadingList.DTO;
using ReadingListApi.Services.ReadingList.Models;
using ReadingListApi.Services.Screenshot;

namespace ReadingListApi.Services.ReadingList
{
    public class ReadingListService : IReadingListService
    {
        private readonly List<ReadingItemModel> _readingItems = new();

        private readonly IScreenshotService _screenshotService;
        private readonly IConfiguration _configuration;

        public ReadingListService(IScreenshotService screenshotService, IConfiguration configuration)
        {
            _screenshotService = screenshotService;
            _configuration = configuration;
        }

        public async Task<Guid> CreateReadingItemAsync(ReadingItemCreateDTO readingItem)
        {
            // Create a new model for the reading item
            var model = new ReadingItemModel()
            {
                Id = Guid.NewGuid(),
                Reminder = readingItem.Reminder,
                Title = readingItem.Title,
                Url = readingItem.Url
            };

            // Determine where to save the screenshot
            var fileName = $"{model.Id}.png";
            var fullFilePath = Path.Combine(_configuration["ScreenshotsFolder"], fileName);

            // Take the screenshot
            var screenshotBytes = await _screenshotService.ScreenshotUrlAsync(readingItem.Url);
            await File.WriteAllBytesAsync(fullFilePath, screenshotBytes);

            // Update our model with the file name
            model.ScreenshotTaken = true;

            // Add the model to our "database"
            _readingItems.Add(model);

            return model.Id;
        }

        public async Task<byte[]?> GetReadingItemScreenshotAsync(Guid id)
        {
            // Try get the item from the database
            var item = _readingItems.FirstOrDefault(x => x.Id == id);

            // Make sure we have selected a reading item record and that
            // a screenshot was taken. If no item was found or the screenshot
            // has not been taken, return null.
            if (item == null || item.ScreenshotTaken == false)
                return null;

            // Retrieve the screenshot and return it as a byte array.
            var fullFilePath = Path.Combine(_configuration["ScreenshotsFolder"], $"{item.Id}.png");
            return await File.ReadAllBytesAsync(fullFilePath);
        }

        public Task<IEnumerable<ReadingItemDTO>> ListReadingItemsAsync()
        {
            // Return all the reading items in the database
            return Task.FromResult(_readingItems.Select(x => new ReadingItemDTO() 
            {
                Id = x.Id,
                Reminder = x.Reminder,
                Title = x.Title,
                Url = x.Url,
                ScreenshotTaken = x.ScreenshotTaken
            }));
        }
    }
}