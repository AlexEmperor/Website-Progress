namespace Website_Progress.Validators
{
    public class EditAdminUserViewModelValidator : AbstractValidator<EditAdminUserViewModel>
    {
        public EditAdminUserViewModelValidator()
        {
            RuleFor(x => x.Email).ValidEmail(5, 30);
            RuleFor(x => x.Phone).ValidPhone();
            RuleFor(x => x.FirstName).ValidFirstName();
            RuleFor(x => x.LastName).ValidLastName();
        }
    }
}