using FluentValidation;

namespace Inz.DTOModel.Validators
{
    public class CalendarDTOValidator : AbstractValidator<CalendarDTO>
    {
        public CalendarDTOValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(c => c.DoctorId).NotEmpty().NotNull().GreaterThan(0);
            RuleFor(c => c.TimeBlockIds).NotEmpty().NotNull().ForEach(x => x.InclusiveBetween(1,5));
            RuleFor(c => c.Date).NotEmpty().NotNull().GreaterThan(DateTime.Now.Date.AddDays(-1));
        }
    }
}
