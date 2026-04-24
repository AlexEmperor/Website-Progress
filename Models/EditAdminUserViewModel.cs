namespace Website_Progress.Models
{
    public class EditAdminUserViewModel
    {
        public string? Id { get; set; }

        [Display(Name = "Логин", Prompt = "Логин")]
        [DataType(DataType.EmailAddress)]
        public required string Email { get; set; }

        [Display(Name = "Телефон", Prompt = "Телефон")]
        [DataType(DataType.PhoneNumber)]
        public required string Phone { get; set; }

        [Display(Name = "Имя", Prompt = "Имя")]
        [DataType(DataType.Text)]
        public required string FirstName { get; set; }

        [Display(Name = "Фамилия", Prompt = "Фамилия")]
        [DataType(DataType.Text)]
        public required string LastName { get; set; }

        public string? Role { get; set; }
    }
}
