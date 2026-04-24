namespace Website_Progress.Validators
{
    public class DeliveryUserViewModelValidator : AbstractValidator<DeliveryUserViewModel>
    {
        public DeliveryUserViewModelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Не указано имя покупателя")
                .Length(2, 25).WithMessage("Имя должно быть от 2 до 25 символов");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Не указан адрес доставки")
                .Length(5, 100).WithMessage("Адрес должен быть от 5 до 100 символов");

            RuleFor(x => x.Phone).ValidPhone();

            RuleFor(x => x.Email).ValidEmail();

            RuleFor(x => x.Telegram)
                .NotEmpty().WithMessage("Не указан профиль Telegram")
                .Length(5, 50).WithMessage("Telegram должен быть от 5 до 50 символов")
                .Matches(@"^@?[A-Za-z0-9_]+$")
                    .WithMessage("Некорректный формат профиля Telegram");

            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("Не указана дата доставки")
                .Must(d => d >= DateOnly.FromDateTime(DateTime.Today))
                    .WithMessage("Дата доставки не может быть в прошлом");

            RuleFor(x => x.Comment)
                .MaximumLength(512).WithMessage("Максимальная длина комментария 512 символов");
        }
    }
}