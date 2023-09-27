using Inz.DTOModel;
using Inz.Model;
using Inz.OneOfHelper;
using OneOf;

namespace Inz.Services
{
    public interface IDoctorVisitService
    {
        public Task<OneOf<DoctorVisit, DatabaseExceptionResponse>> GetDoctorVisitByIdAsync(int visitId);
        public Task<OneOf<OkResponse, NotFoundResponse, DatabaseExceptionResponse>> BookDoctorVisitAsync(DoctorVisitDTO doctorVisitDTO);
        public Task<OneOf<OkResponse, NotFoundResponse, DatabaseExceptionResponse>> RemoveDoctorVisitAsync(int doctorVisitId, int patientId);
    }
}