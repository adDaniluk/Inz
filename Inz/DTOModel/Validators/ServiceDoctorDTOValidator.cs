using FluentValidation;

namespace Inz.DTOModel.Validators
{
    public class ServiceDoctorDTOValidator : AbstractValidator<DoctorServiceDTO>
    {
        public ServiceDoctorDTOValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(s => s.ServiceId).NotEmpty().NotNull();
            RuleFor(s => s.Price).NotEmpty().NotNull().GreaterThan(0); 
        }
    }
}
