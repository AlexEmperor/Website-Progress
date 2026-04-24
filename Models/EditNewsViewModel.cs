namespace Website_Progress.Models
{
    public class EditNewsViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Название новости")]
        public string Title { get; set; }

        [Display(Name = "Описание новости")]
        public string Description { get; set; }

        public string? ImagePath { get; set; }

        [Display(Name = "Дата")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Display(Name = "Показывать на главной")]
        public bool IsOnMainPage { get; set; }

        public IFormFile? ImageFile { get; set; }
    }
}
