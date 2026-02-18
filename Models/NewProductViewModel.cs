using System.ComponentModel.DataAnnotations;

namespace Website_Progress.Models
{
    public class NewProductViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Наименование товара", Prompt = "Наименование товара")]
        [Required(ErrorMessage = "Не указано наименование товара")]
        [DataType(DataType.Text)]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Наименование товара должно быть от {2} до {1} символов")]
        public string Name { get; set; }

        [Display(Name = "Цена, руб.", Prompt = "Цена, руб.")]
        [Required(ErrorMessage = "Не указана цена товара")]
        [Range(0, 1000000, ErrorMessage = "Цена товара должна быть в диапазоне от {1} до {2} рублей")]
        public decimal Cost { get; set; }

        [Display(Name = "Описание товара", Prompt = "Описание товара")]
        [MaxLength(4096, ErrorMessage = "Максимальная длина описания товара {1} символов")]
        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }

        [Required]
        public string? PhotoPath { get; set; } //= "/img/product.png";

        public string? PresentationPath { get; set; }

        public NewProductViewModel()
        {

        }

        public NewProductViewModel(int id, string name, decimal cost, string? description, string? photoPath,
            string? presentationPath)
        {
            Id = id;
            Name = name;
            Description = description;
            Cost = cost;
            PhotoPath = photoPath;
            PresentationPath = presentationPath;
        }
    }
}
