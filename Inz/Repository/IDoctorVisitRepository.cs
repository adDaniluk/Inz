using Inz.Model;
using Inz.OneOfHelper;
using OneOf;

namespace Inz.Repository
{
    public interface IDoctorVisitRepository
    {
        public Task<OneOf<OkResponse, DatabaseExceptionResponse>> CreateDoctorVisitAsync(DoctorVisit doctorVisit);
        public Task<OneOf<DoctorVisit?, DatabaseExceptionResponse>> GetDoctorVisitAsync(int doctorVisitId);
        public Task<OneOf<List<DoctorVisit>, DatabaseExceptionResponse>> GetDoctorVisitListAsync(int doctorId);
    }
}
