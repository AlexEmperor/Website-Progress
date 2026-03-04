using System.ComponentModel.DataAnnotations;

namespace Website_Progress.Models
{
    public class NewsViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Название новости")]
        [Required(ErrorMessage = "Не указано название новости")]
        public string Title { get; set; }

        //[Display(Name = "Описание новости")]
        //[Required(ErrorMessage = "Не указано короткое описание новости")]
        //public string ShortText { get; set; }

        [Display(Name = "Описание новости")]
        [Required(ErrorMessage = "Не указано описание новости")]
        public string Description { get; set; }

        //[Required(ErrorMessage = "Не указана картинка")]
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

        public NewsViewModel(int id, string title, string shortText, string imageUrl, DateTime date)
        {
            Id = id;
            Title = title;
            Description = shortText;
            ImagePath = imageUrl;
            Date = date;
        }
    }

}
