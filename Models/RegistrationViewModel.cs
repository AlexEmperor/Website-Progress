namespace Website_Progress.Models
{
    public class RegistrationViewModel
    {
        [Display(Name = "Логин", Prompt = "Ваш логин")]
        [DataType(DataType.Text)]
        public required string Login { get; set; }

        [Display(Name = "Электронная почта", Prompt = "Ваша электронная почта")]
        [DataType(DataType.EmailAddress)]
        public required string Email { get; set; }

        [Display(Name = "Пароль", Prompt = "Ваш пароль")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [Display(Name = "Подтвердите пароль", Prompt = "Подтвердите пароль")]
        [DataType(DataType.Password)]
        public required string ConfirmPassword { get; set; }

        [Display(Name = "Телефон", Prompt = "Ваш телефон")]
        [DataType(DataType.PhoneNumber)]
        public required string Phone { get; set; }

        [Display(Name = "Имя", Prompt = "Ваше имя")]
        [DataType(DataType.Text)]
        public required string FirstName { get; set; }

        [Display(Name = "Фамилия", Prompt = "Ваша фамилия")]
        [DataType(DataType.Text)]
        public required string LastName { get; set; }

        public DateTime CreationDateTime => DateTime.Now;
    }
}
