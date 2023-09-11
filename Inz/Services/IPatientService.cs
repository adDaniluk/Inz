using Inz.DTOModel;
using Inz.OneOfHelper;
using OneOf;

namespace Inz.Services
{
    public interface IPatientService
    {
        public Task<OneOf<OkResponse, AlreadyExistResponse, DatabaseExceptionResponse>> InsertPatientAsync(PatientDTO patientDTO);
        public Task<OneOf<OkResponse, NotFoundResponse, DatabaseExceptionResponse>> UpdatePatientAsyc(UpdatePatientDTO updatePatientDTO);
    }
}
