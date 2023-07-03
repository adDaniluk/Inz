using FluentValidation;

namespace Inz.DTOModel.Validators
{
    public class PatientDTOValidator : AbstractValidator<PatientDTO>
    {
        public PatientDTOValidator()
        {
            RuleFor(p => p.Login).NotNull().NotEmpty().Length(5, 50);
            RuleFor(p => p.Password).NotNull().NotEmpty().Length(8, 100);
            RuleFor(p => p.Name).NotNull().NotEmpty().Length(3, 200);
            RuleFor(p => p.Surname).NotNull().NotEmpty().Length(3, 200);
            RuleFor(p => p.UserId).NotNull().NotEmpty().Length(11);
            RuleFor(p => p.DateOfBirth).NotNull().NotEmpty().LessThan(DateTime.Now.Date);
            RuleFor(p => p.City).NotNull().NotEmpty().Length(3, 100);
            RuleFor(p => p.Email).NotNull().NotEmpty().EmailAddress();
            RuleFor(P => P.PostCode).NotNull().NotEmpty().Length(6).Matches("[0-9]{2}-[0-9]{3}");
            RuleFor(p => p.Phone).NotNull().NotEmpty().InclusiveBetween(100000000, 999999999);
            RuleFor(p => p.Street).NotNull().NotEmpty().Length(3, 200);
            RuleFor(p => p.AparmentNumber).NotEmpty().NotNull();
            RuleFor(p => p.PostCode).NotNull().NotEmpty().Length(6);
        }
    }
}
