using FluentValidation;
using Inz.Model;

namespace Inz.DTOModel.Validators
{
    public class LoginDTOValidator : AbstractValidator<LoginDTO>
    {
        public LoginDTOValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(p => p.Login).NotNull().NotEmpty().Length(5, 50);
            RuleFor(p => p.Password).NotNull().NotEmpty().Length(8, 100);
            RuleFor(p => p.PersonType).NotNull().Must(x => x == PersonType.Doctor || x == PersonType.Patient).WithMessage("PersonType must be a doctor or a patient.");
        }
    }
}
