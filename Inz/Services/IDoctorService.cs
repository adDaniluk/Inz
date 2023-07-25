using Inz.DTOModel;
using Inz.OneOfHelper;
using OneOf;

namespace Inz.Services
{
    public interface IDoctorService
    {
        public Task<OneOf<OkResponse, NotValidateResponse, DatabaseExceptionResponse>> InsertDoctorAsync(DoctorDTO doctorDTO);
        public Task<OneOf<OkResponse, NotFoundResponse, NotValidateResponse, DatabaseExceptionResponse>> UpdateDoctorAsync(UpdateDoctorDTO updateDoctorDTO);
        public Task<OneOf<OkResponse, NotFoundResponse, DatabaseExceptionResponse>> AddDoctorServiceAsync(ServiceDoctorDTO serviceDTO);
        public Task<OneOf<OkResponse, NotFoundResponse, DatabaseExceptionResponse>> RemoveDoctorServiceAsync(ServiceDoctorDTO serviceDTO);
    }
}
