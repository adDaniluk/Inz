using FluentValidation;

namespace Inz.DTOModel.Validators
{
    public class DoctorVisitDTOValidator : AbstractValidator<DoctorVisitDTO>
    {
        public DoctorVisitDTOValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.CalendarId).NotEmpty().NotNull().GreaterThan(0);
            RuleFor(x => x.PatientId).NotEmpty().NotNull().GreaterThan(0);
            RuleFor(x => x.DoctorServiceId).NotEmpty().NotNull().GreaterThan(0);
        }
    }
}
