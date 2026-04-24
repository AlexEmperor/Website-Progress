namespace Website_Progress.Models
{
    public class AdminUserViewModel
    {
        public string? Id { get; set; }

        [Display(Name = "Логин", Prompt = "Логин")]
        [DataType(DataType.EmailAddress)]
        public required string Login { get; set; }

        [Display(Name = "Пароль", Prompt = "Пароль")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Display(Name = "Телефон", Prompt = "Телефон")]
        [DataType(DataType.PhoneNumber)]
        public required string Phone { get; set; }

        [Display(Name = "Имя", Prompt = "Имя")]
        [DataType(DataType.Text)]
        public required string FirstName { get; set; }

        [Display(Name = "Фамилия", Prompt = "Фамилия")]
        [DataType(DataType.Text)]
        public required string LastName { get; set; }

        public DateTime CreationDateTime { get; set; }

        public string? Email { get; set; }

        public string? Role { get; set; }
    }
}
