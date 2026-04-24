namespace Website_Progress.Models
{
    public class NewsViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Название новости")]
        [Required(ErrorMessage = "Не указано название новости")]
        public string Title { get; set; }

        [Display(Name = "Описание новости")]
        public string? Description { get; set; }

        public string? ImagePath { get; set; }

        [Display(Name = "Дата")]
        [Required(ErrorMessage = "Не указана дата")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Display(Name = "Показывать на главной")]
        public bool IsOnMainPage { get; set; }

        public IFormFile? ImageFile { get; set; }

        public NewsViewModel()
        {

        }
    }

}
