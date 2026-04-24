namespace Website_Progress.Validators
{
    public class ProductViewModelValidator : AbstractValidator<ProductViewModel>
    {
        public ProductViewModelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Не указано наименование товара")
                .Length(2, 200).WithMessage("Наименование товара должно быть от 2 до 200 символов");

            RuleFor(x => x.Cost)
                .InclusiveBetween(0, 1_000_000)
                    .WithMessage("Цена товара должна быть от 0 до 1 000 000 рублей");

            RuleFor(x => x.Description)
                .MaximumLength(16384).WithMessage("Максимальная длина описания товара 16384 символов");

            RuleFor(x => x.ShortDescription)
                .MaximumLength(250).WithMessage("Максимальная длина краткого описания 250 символов");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Некорректный статус товара");

            When(x => x.PhotoFile != null, () =>
            {
                RuleFor(x => x.PhotoFile!)
                    .Must(BeValidImage).WithMessage("Допускаются только изображения (jpg, png, webp, gif)")
                    .Must(f => f.Length <= 5 * 1024 * 1024)
                        .WithMessage("Размер изображения не должен превышать 5 МБ");
            });

            When(x => x.PhotoFiles != null && x.PhotoFiles.Count > 0, () =>
            {
                RuleForEach(x => x.PhotoFiles!)
                    .Must(BeValidImage).WithMessage("Допускаются только изображения (jpg, png, webp, gif)")
                    .Must(f => f.Length <= 5 * 1024 * 1024)
                        .WithMessage("Размер изображения не должен превышать 5 МБ");
            });

            When(x => x.PresentationFile != null, () =>
            {
                RuleFor(x => x.PresentationFile!)
                    .Must(f => Path.GetExtension(f.FileName)
                                   .Equals(".pptx", StringComparison.OrdinalIgnoreCase))
                        .WithMessage("Презентация должна быть в формате .pptx")
                    .Must(f => f.Length <= 50 * 1024 * 1024)
                        .WithMessage("Размер презентации не должен превышать 50 МБ");
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