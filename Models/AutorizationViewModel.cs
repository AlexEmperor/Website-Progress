namespace Website_Progress.Models
{
    public class AutorizationViewModel
    {
        [Display(Name = "Логин", Prompt = "Ваш логин")]
        [DataType(DataType.EmailAddress)]
        public required string Login { get; set; }

        [Display(Name = "Пароль", Prompt = "Ваш пароль")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [Display(Name = "Запомнить меня")]
        public bool Memorize { get; set; }
    }
}
