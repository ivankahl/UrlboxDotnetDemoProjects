namespace ReadingListApi.Services.ReadingList.DTO
{
    public class ReadingItemDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime Reminder { get; set; }
        public string Url { get; set; }
        public bool ScreenshotTaken { get; set; }
    }
}