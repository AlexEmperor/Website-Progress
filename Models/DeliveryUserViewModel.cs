using System.ComponentModel.DataAnnotations;

namespace Website_Progress.Models
{
    public class DeliveryUserViewModel
    {
        public Guid Id { get; set; }


        [DataType(DataType.Text)]
        [Display(Name = "Имя покупателя", Prompt = "Ваше имя")]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "Логин должен быть от {2} до {1} символов")]
        [Required(ErrorMessage = "Не указано имя покупателя")]
        public string Name { get; set; }


        [Display(Name = "Адрес доставки", Prompt = "Ваш адрес")]
        [DataType(DataType.Text)]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Логин должен быть от {2} до {1} символов")]
        [Required(ErrorMessage = "Не указан адрес доставки")]
        public string Address { get; set; }

        [Display(Name = "Телефон", Prompt = "Ваш телефон")]
        [StringLength(16, MinimumLength = 5, ErrorMessage = "Логин должен быть от {2} до {1} символов")]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Не указан телефон покупателя")]
        public string Phone { get; set; }

        [Display(Name = "Электронная почта", Prompt = "Ваша электронная почта")]
        [StringLength(50, MinimumLength = 5/*, ErrorMessage = "Логин должен быть от {2} до {1} символов"*/)]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Не указана электронная почта")]
        public string Email { get; set; }

        [Display(Name = "Telegram", Prompt = "@telegramUserName")]
        [StringLength(50, MinimumLength = 5/*, ErrorMessage = "Логин должен быть от {2} до {1} символов"*/)]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Не указан профиль Telegram")]
        public string Telegram { get; set; }

        [Display(Name = "Дата доставки")]
        [Required(ErrorMessage = "Не указана дата доставки")]
        [DataType(DataType.Date)]
        public DateOnly Date { get; set; }

        [Display(Name = "Комментарий", Prompt = "Ваш комментарий")]
        [MaxLength(512, ErrorMessage = "Максимальная длина комментария {1} символов")]
        [DataType(DataType.MultilineText)]
        public string? Comment { get; set; }

    }
}
