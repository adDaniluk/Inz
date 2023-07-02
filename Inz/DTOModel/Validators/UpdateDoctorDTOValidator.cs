using FluentValidation;

namespace Inz.DTOModel.Validators
{
    public class UpdateDoctorDTOValidator : AbstractValidator<UpdateDoctorDTO>
    {
        public UpdateDoctorDTOValidator()
        {
            RuleFor(p => p.Id).NotEmpty().NotNull();
            RuleFor(p => p.City).NotNull().NotEmpty().Length(3, 100);
            RuleFor(p => p.Email).NotNull().NotEmpty().EmailAddress();
            RuleFor(P => P.PostCode).NotNull().NotEmpty().Length(6).Matches("[0-9]{2}-[0-9]{3}");
            RuleFor(p => p.Phone).NotNull().NotEmpty().InclusiveBetween(100000000, 999999999);
            RuleFor(p => p.Street).NotNull().NotEmpty().Length(3, 200);
            RuleFor(p => p.AparmentNumber).NotEmpty().NotNull();
            RuleFor(p => p.PostCode).NotNull().NotEmpty().Length(6);
            //TODO: Update List of MedicalSpecializations
        }
    }
}
