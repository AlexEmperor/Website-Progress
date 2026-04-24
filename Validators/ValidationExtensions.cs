namespace Website_Progress.Validators
{
    public static class ValidationExtensions
    {
        public static IRuleBuilderOptions<T, string> ValidEmail<T>(
            this IRuleBuilder<T, string> rule, int min = 5, int max = 50)
        {
            return rule
                .NotEmpty().WithMessage("Не указана почта")
                .EmailAddress().WithMessage("Введите валидный email")
                .Length(min, max).WithMessage($"Email должен быть от {min} до {max} символов");
        }

        public static IRuleBuilderOptions<T, string> ValidPhone<T>(
            this IRuleBuilder<T, string> rule)
        {
            return rule
                .NotEmpty().WithMessage("Не указан телефон")
                .Length(5, 16).WithMessage("Телефон должен быть от 5 до 16 символов")
                .Matches(@"^[\d\+\-\(\)\s]+$")
                    .WithMessage("Телефон содержит недопустимые символы");
        }

        public static IRuleBuilderOptions<T, string> ValidPassword<T>(
            this IRuleBuilder<T, string> rule)
        {
            return rule
                .NotEmpty().WithMessage("Не указан пароль")
                .Length(6, 50).WithMessage("Пароль должен быть от 6 до 50 символов")
                .Matches(@"[0-9]").WithMessage("Пароль должен содержать хотя бы одну цифру")
                .Matches(@"[A-Z]").WithMessage("Пароль должен содержать хотя бы одну заглавную букву")
                .Matches(@"[a-z]").WithMessage("Пароль должен содержать хотя бы одну строчную букву");
        }

        public static IRuleBuilderOptions<T, string> ValidFirstName<T>(
            this IRuleBuilder<T, string> rule)
        {
            return rule
                .NotEmpty().WithMessage("Не указано имя")
                .Length(2, 25).WithMessage("Имя должно быть от 2 до 25 символов");
        }

        public static IRuleBuilderOptions<T, string> ValidLastName<T>(
            this IRuleBuilder<T, string> rule)
        {
            return rule
                .NotEmpty().WithMessage("Не указана фамилия")
                .Length(2, 25).WithMessage("Фамилия должна быть от 2 до 25 символов");
        }
    }
}