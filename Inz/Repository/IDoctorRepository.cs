using Inz.DTOModel;
using Inz.Model;
using Inz.OneOfHelper;
using OneOf;

namespace Inz.Repository
{
    public interface IDoctorRepository
    {
        public Task<OneOf<OkResponse, DatabaseExceptionResponse>> InsertDoctorAsync(Doctor doctor);
        public Task<OneOf<Doctor, NotFoundResponse, DatabaseExceptionResponse>> GetDoctorAsync(int id);
        public Task<OneOf<OkResponse, DatabaseExceptionResponse>> UpdateDoctorAsync(Doctor doctor);
        public Task<OneOf<OkResponse, NotFoundResponse, DatabaseExceptionResponse>> AddDoctorServiceAsync(ServiceDoctorDTO serviceDTO);
        public Task<OneOf<OkResponse, NotFoundResponse, DatabaseExceptionResponse>> RemoveDoctorServiceAsync(ServiceDoctorDTO serviceDTO);
    }
}
