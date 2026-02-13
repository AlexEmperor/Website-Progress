namespace Website_Progress.Models
{
    public class NewsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortText { get; set; }
        public string ImageUrl { get; set; }
        public DateOnly Date { get; set; }

        public NewsViewModel(int id, string title, string shortText, string imageUrl, DateOnly date)
        {
            Id = id;
            Title = title;
            ShortText = shortText;
            ImageUrl = imageUrl;
            Date = date;
        }
    }

}
