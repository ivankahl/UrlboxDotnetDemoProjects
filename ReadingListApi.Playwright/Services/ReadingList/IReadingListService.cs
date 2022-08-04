using ReadingListApi.Services.ReadingList.DTO;

namespace ReadingListApi.Services.ReadingList
{
    public interface IReadingListService
    {
        Task<Guid> CreateReadingItemAsync(ReadingItemCreateDTO readingItem);
        Task<byte[]?> GetReadingItemScreenshotAsync(Guid id);
        Task<IEnumerable<ReadingItemDTO>> ListReadingItemsAsync();
    }
}