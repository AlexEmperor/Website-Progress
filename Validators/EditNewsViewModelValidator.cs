namespace Website_Progress.Validators
{
    public class EditNewsViewModelValidator : AbstractValidator<EditNewsViewModel>
    {
        public EditNewsViewModelValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Не указано название новости")
                .MaximumLength(200).WithMessage("Название не должно превышать 200 символов");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Не указано описание новости")
                .MaximumLength(16384).WithMessage("Описание не должно превышать 16384 символов");

            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("Не указана дата");

            When(x => x.ImageFile != null, () =>
            {
                RuleFor(x => x.ImageFile!)
                    .Must(BeValidImage).WithMessage("Допускаются только изображения (jpg, png, webp, gif)")
                    .Must(f => f.Length <= 5 * 1024 * 1024)
                        .WithMessage("Размер изображения не должен превышать 5 МБ");
            });
        }

        private static bool BeValidImage(IFormFile file)
        {
            var allowed = new[] { ".jpg", ".jpeg", ".png", ".webp", ".gif" };
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            return allowed.Contains(ext);
        }
    }
}