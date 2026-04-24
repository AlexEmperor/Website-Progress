namespace Website_Progress.Validators
{
    public class RegistrationViewModelValidator : AbstractValidator<RegistrationViewModel>
    {
        public RegistrationViewModelValidator()
        {
            RuleFor(x => x.Login)
                .NotEmpty().WithMessage("Не указан логин")
                .Length(5, 50).WithMessage("Логин должен быть от 5 до 50 символов");

            RuleFor(x => x.Email).ValidEmail();
            RuleFor(x => x.Password).ValidPassword();

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Не указан повторный пароль")
                .Equal(x => x.Password).WithMessage("Пароли не совпадают");

            RuleFor(x => x.Phone).ValidPhone();
            RuleFor(x => x.FirstName).ValidFirstName();
            RuleFor(x => x.LastName).ValidLastName();
        }
    }
}