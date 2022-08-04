namespace ReadingListApi.Services.ReadingList.Models
{
    public class ReadingItemModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime Reminder { get; set; }
        public string Url { get; set; }
        public bool ScreenshotTaken { get; set; }
    }
}