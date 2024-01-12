using FluentValidation;

namespace Inz.DTOModel.Validators
{
    public class RemoveDoctorServiceDTOValidator : AbstractValidator<RemoveDoctorServiceDTO>
    {
        public RemoveDoctorServiceDTOValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(s => s.ServiceId).NotEmpty().NotNull();
        }
    }
}
