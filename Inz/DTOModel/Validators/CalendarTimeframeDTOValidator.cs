using FluentValidation;
using System.Globalization;
namespace Inz.DTOModel.Validators
{
    public class CalendarTimeframeDTOValidator : AbstractValidator<CalendarTimeframeDTO>
    {
        public CalendarTimeframeDTOValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(date => date.StartDate).NotEmpty()
                .NotNull()
                .LessThanOrEqualTo(date => date.EndDate)
                .Must(date => BeValidDate(date)).WithMessage("For StartDate please provide correct datetime format. Example: 2003-12-01.");
            RuleFor(date => date.EndDate).NotEmpty()
                .NotNull()
                .GreaterThanOrEqualTo(date => date.StartDate)
                .Must(date => BeValidDate(date)).WithMessage("For EndDate please provide correct datetime format. Example: 2003-12-01.");
        }

        private static bool BeValidDate(string value)
        {
            return DateTime.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date);
        }
    }
}
