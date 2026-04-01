using System.ComponentModel.DataAnnotations;

namespace Website_Progress.Models
{
    public class AutorizationViewModel
    {
        [Display(Name = "Логин", Prompt = "Ваш логин")]
        [Required(ErrorMessage = "Не указан логин")]
        [DataType(DataType.EmailAddress)]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "Логин должен быть от {2} до {1} символов")]
        public required string Login { get; set; }

        //[Display(Name = "Электронная почта", Prompt = "Ваша электронная почта")]
        //[Required(ErrorMessage = "Не указана почта")]
        //[EmailAddress(ErrorMessage = "Введите валидный email")]
        //[StringLength(50, MinimumLength = 5, ErrorMessage = "Логин должен быть от {2} до {1} символов")]
        //public required string Email { get; set; }

        [Display(Name = "Пароль", Prompt = "Ваш пароль")]
        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Пароль должен быть от {2} до {1} символов")]
        public required string Password { get; set; }

        [Display(Name = "Запомнить меня")]
        public bool Memorize { get; set; }
    }
}
