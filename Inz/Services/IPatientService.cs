using Inz.DTOModel;
using Inz.OneOfHelper;
using OneOf;

namespace Inz.Services
{
    public interface IPatientService
    {
        public Task<OneOf<OkResponse, NotValidateResponse, DatabaseExceptionResponse>> InsertPatientAsync(PatientDTO patientDTO);
        public Task<OneOf<OkResponse, NotFoundResponse, NotValidateResponse, DatabaseExceptionResponse>> UpdatePatientAsyc(UpdatePatientDTO updatePatientDTO);
    }
}
