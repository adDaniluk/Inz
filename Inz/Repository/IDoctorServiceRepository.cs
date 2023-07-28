using Inz.Model;
using Inz.OneOfHelper;
using OneOf;

namespace Inz.Repository
{
    public interface IDoctorServiceRepository
    {
        public Task<OneOf<DoctorServices, NotFoundResponse, DatabaseExceptionResponse>> GetDoctorServiceAsync(int doctorId, int serviceId);
        public Task<OneOf<IList<DoctorServices>, NotFoundResponse, DatabaseExceptionResponse>> GetDoctorServiceByDoctorIdAsync(int id);
        public Task<OneOf<OkResponse, DatabaseExceptionResponse>> AddDoctorServiceAsync(DoctorServices doctorService);
        public Task<OneOf<OkResponse, DatabaseExceptionResponse>> RemoveDoctorServiceAsync(DoctorServices doctorServices);
    }
}
