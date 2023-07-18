using FluentValidation;

namespace Inz.DTOModel.Validators
{
    public class RemoveServiceDoctorDTOValidator : AbstractValidator<ServiceDoctorDTO>
    {
        public RemoveServiceDoctorDTOValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(s => s.ServiceId).NotEmpty().NotNull();
            RuleFor(s => s.DoctorId).NotEmpty().NotNull(); 
        }
    }
}
