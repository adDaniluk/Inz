using Inz.DTOModel;

namespace Inz.Services
{
    public interface IPatientService
    {
        public Task InsertPatientAsync(PatientDTO patientDTO);
        public Task ValidateUser();
    }
}
