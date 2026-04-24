namespace Website_Progress.Validators
{
    public class AdminUserViewModelValidator : AbstractValidator<AdminUserViewModel>
    {
        public AdminUserViewModelValidator()
        {
            RuleFor(x => x.Login).ValidEmail(5, 30);

            When(x => !string.IsNullOrEmpty(x.Password), () =>
            {
                RuleFor(x => x.Password!).ValidPassword();
            });

            RuleFor(x => x.Phone).ValidPhone();
            RuleFor(x => x.FirstName).ValidFirstName();
            RuleFor(x => x.LastName).ValidLastName();
        }
    }
}