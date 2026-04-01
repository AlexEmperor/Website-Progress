using System.ComponentModel.DataAnnotations;

namespace Website_Progress.Models
{
    public class RegistrationViewModel
    {
        [Display(Name = "Логин", Prompt = "Ваш логин")]
        [Required(ErrorMessage = "Не указан логин")]
        [DataType(DataType.Text)]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Логин должен быть от {2} до {1} символов")]
        public required string Login { get; set; }

        [Display(Name = "Электронная почта", Prompt = "Ваша электронная почта")]
        [Required(ErrorMessage = "Не указана почта")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Введите валидный email")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Логин должен быть от {2} до {1} символов")]
        public required string Email { get; set; }

        [Display(Name = "Пароль", Prompt = "Ваш пароль")]
        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Пароль должен быть от {2} до {1} символов")]
        public required string Password { get; set; }


        [Display(Name = "Подтвердите пароль", Prompt = "Подтвердите пароль")]
        [Required(ErrorMessage = "Не указан повторный пароль")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public required string ConfirmPassword { get; set; }


        [Display(Name = "Телефон", Prompt = "Ваш телефон")]
        [Required(ErrorMessage = "Не указан телефон")]
        [DataType(DataType.PhoneNumber)]
        [Phone(ErrorMessage = "Телефон может содержать только цифры")]
        [StringLength(16, MinimumLength = 5, ErrorMessage = "Телефон должен быть от {2} до {1} символов")]
        public required string Phone { get; set; }


        [Display(Name = "Имя", Prompt = "Ваше имя")]
        [Required(ErrorMessage = "Не указано имя")]
        [DataType(DataType.Text)]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "Имя должно быть от {2} до {1} символов")]
        public required string FirstName { get; set; }


        [Display(Name = "Фамилия", Prompt = "Ваша фамилия")]
        [Required(ErrorMessage = "Не указана фамилия")]
        [DataType(DataType.Text)]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "Фамилия должна быть от {2} до {1} символов")]
        public required string LastName { get; set; }

        public DateTime CreationDateTime => DateTime.Now;
    }
}
