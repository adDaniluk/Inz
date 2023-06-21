using Inz.DTOModel;

namespace Inz.Services
{
    public interface IUserServices
    {
        public Task RegisterUser(PatientDTO patientDTO, AddressDTO addressDTO);
        public Task ValidateUser();
    }
}
