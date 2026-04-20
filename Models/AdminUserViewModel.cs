using System.ComponentModel.DataAnnotations;

namespace Website_Progress.Models
{
    public class AdminUserViewModel
    {
        public string? Id { get; set; }


        [Display(Name = "Логин", Prompt = "Логин")]
        [Required(ErrorMessage = "Не указан логин пользователя")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Введите валидный email")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "Логин должен быть от {2} до {1} символов")]
        public required string Login { get; set; }


        [Display(Name = "Пароль", Prompt = "Пароль")]
        [Required(ErrorMessage = "Не указан пароль пользователя")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Пароль должен быть от {2} до {1} символов")]
        public string? Password { get; set; }


        [Display(Name = "Телефон", Prompt = "Телефон")]
        [Required(ErrorMessage = "Не указан телефон пользователя")]
        [DataType(DataType.PhoneNumber)]
        [Phone(ErrorMessage = "Телефон может содержать только цифры")]
        [StringLength(16, MinimumLength = 5, ErrorMessage = "Телефон должен быть от {2} до {1} символов")]
        public required string Phone { get; set; }


        [Display(Name = "Имя", Prompt = "Имя")]
        [Required(ErrorMessage = "Не указано имя пользователя")]
        [DataType(DataType.Text)]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "Имя должно быть от {2} до {1} символов")]
        public required string FirstName { get; set; }


        [Display(Name = "Фамилия", Prompt = "Фамилия")]
        [Required(ErrorMessage = "Не указана фамилия пользователя")]
        [DataType(DataType.Text)]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "Фамилия должна быть от {2} до {1} символов")]
        public required string LastName { get; set; }


        public DateTime CreationDateTime { get; set; }

        public string? Email { get; set; }

        public string? Role { get; set; }
    }
}
