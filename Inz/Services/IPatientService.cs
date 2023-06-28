using Inz.DTOModel;
using Inz.Model;
using Inz.OneOfHelper;
using OneOf;
using OneOf.Types;

namespace Inz.Services
{
    public interface IPatientService
    {
        public Task<OneOf<Patient, DatabaseException>> InsertPatientAsync(PatientDTO patientDTO);

        public Task<OneOf<Patient, NotFound, DatabaseException>> UpdatePatientAsync(PatientDTO patientDTO, int id);
    }
}
