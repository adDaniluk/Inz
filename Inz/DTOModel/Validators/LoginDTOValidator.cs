using FluentValidation;

namespace Inz.DTOModel.Validators
{
    public class LoginDTOValidator : AbstractValidator<LoginDTO>
    {
        public LoginDTOValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(p => p.Login).NotNull().NotEmpty().Length(5, 50);
            RuleFor(p => p.Password).NotNull().NotEmpty().Length(8, 100);
            RuleFor(p => p.PersonType).NotNull().IsInEnum();
        }
    }
}
