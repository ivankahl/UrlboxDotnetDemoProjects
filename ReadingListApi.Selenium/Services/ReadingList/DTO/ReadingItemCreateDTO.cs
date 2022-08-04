namespace ReadingListApi.Services.ReadingList.DTO
{
    public class ReadingItemCreateDTO
    {
        public string Title { get; set; }
        public DateTime Reminder { get; set; }
        public string Url { get; set; }
    }
}