namespace Website_Progress.ModelsDTO
{
    public class News
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortText { get; set; }
        public string Description { get; set; }

        public string? ImageUrl { get; set; }
        public DateTime Date { get; set; }
        public bool IsOnMainPage { get; set; }
    }

}
