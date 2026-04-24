namespace Website_Progress.Models
{
    public class DeliveryUserViewModel
    {
        public Guid Id { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Имя покупателя", Prompt = "Ваше имя")]
        public string Name { get; set; }

        [Display(Name = "Адрес доставки", Prompt = "Ваш адрес")]
        [DataType(DataType.Text)]
        public string Address { get; set; }

        [Display(Name = "Телефон", Prompt = "Ваш телефон")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Display(Name = "Электронная почта", Prompt = "Ваша электронная почта")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Telegram", Prompt = "@telegramUserName")]
        [DataType(DataType.Text)]
        public string Telegram { get; set; }

        [Display(Name = "Дата доставки")]
        [DataType(DataType.Date)]
        public DateOnly Date { get; set; }

        [Display(Name = "Комментарий", Prompt = "Ваш комментарий")]
        [DataType(DataType.MultilineText)]
        public string? Comment { get; set; }

    }
}
