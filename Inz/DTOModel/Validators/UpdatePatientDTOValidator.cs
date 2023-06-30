using FluentValidation;

namespace Inz.DTOModel.Validators
{
    public class UpdatePatientDTOValidator : AbstractValidator<UpdatePatientDTO>
    {
        public UpdatePatientDTOValidator()
        { 
            RuleFor(p => p.Id).NotEmpty();
            RuleFor(p => p.City).NotNull().MaximumLength(100);
            RuleFor(p => p.Email).NotNull().MaximumLength(100);
            RuleFor(P => P.PostCode).NotNull().MaximumLength(6);
            RuleFor(p => p.Phone).NotNull().GreaterThan(500000000).LessThan(1000000000);
            RuleFor(p => p.Street).NotNull().MaximumLength(100);
            RuleFor(p => p.AparmentNumber).NotNull();
            RuleFor(p => p.PostCode).NotNull().Length(6);
        }
    }
}
