namespace Website_Progress.Validators
{
    public class AutorizationViewModelValidator : AbstractValidator<AutorizationViewModel>
    {
        public AutorizationViewModelValidator()
        {
            RuleFor(x => x.Login)
                .NotEmpty().WithMessage("Не указан логин")
                .Length(5, 30).WithMessage("Логин должен быть от 5 до 30 символов");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Не указан пароль");
        }
    }
}