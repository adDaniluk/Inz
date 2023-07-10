using FluentValidation;

namespace Inz.DTOModel.Validators
{
    public class UpdatePatientDTOValidator : AbstractValidator<UpdatePatientDTO>
    {
        public UpdatePatientDTOValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(p => p.Id).NotEmpty().NotNull();
            RuleFor(p => p.City).NotNull().NotEmpty().Length(3, 100);
            RuleFor(p => p.Email).NotNull().NotEmpty().EmailAddress();
            RuleFor(P => P.PostCode).NotNull().NotEmpty().Length(6).Matches("[0-9]{2}-[0-9]{3}").WithMessage("Please provide a valid postcode - example: 03-034.");
            RuleFor(p => p.Phone).NotNull().NotEmpty().InclusiveBetween(100000000, 999999999).WithMessage("Please provide your number phone - it should have 9 digits");
            RuleFor(p => p.Street).NotNull().NotEmpty().Length(3, 200);
            RuleFor(p => p.AparmentNumber).NotEmpty().NotNull();
        }
    }
}
