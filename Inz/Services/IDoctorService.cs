using Inz.DTOModel;
using Inz.OneOfHelper;
using OneOf;

namespace Inz.Services
{
    public interface IDoctorService
    {
        public Task<OneOf<OkResponse, DatabaseExceptionResponse>> InsertDoctorAsync(DoctorDTO doctorDTO);
        public Task<OneOf<OkResponse, NotFoundResponse, DatabaseExceptionResponse>> UpdateDoctorAsync(UpdateDoctorDTO updateDoctorDTO);
        public Task<OneOf<OkResponse, NotFoundResponse, DatabaseExceptionResponse>> AddDoctorServiceAsync(DoctorServiceDTO doctorServiceDTO);
        public Task<OneOf<OkResponse, NotFoundResponse, DatabaseExceptionResponse>> RemoveDoctorServiceAsync(RemoveDoctorServiceDTO removeDoctorServiceDTO);
    }
}
