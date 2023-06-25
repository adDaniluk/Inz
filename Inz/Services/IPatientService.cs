using Inz.DTOModel;
using Inz.Model;
using Inz.OneOfHelper;
using OneOf;

namespace Inz.Services
{
    public interface IPatientService
    {
        public Task<OneOf<Patient, DisconnectFromDatabase>> InsertPatientAsync(PatientDTO patientDTO);
        public Task ValidateUser();
    }
}
