using Inz.Context;
using Inz.Model;
using Inz.OneOfHelper;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace Inz.Repository
{
    public class DoctorVisitRepository : IDoctorVisitRepository
    {
        private readonly DbContextApi _contextApi;
        public DoctorVisitRepository(DbContextApi contextApi)
        {
            _contextApi = contextApi;
        }

        public async Task<OneOf<OkResponse, DatabaseExceptionResponse>> CreateDoctorVisitAsync(DoctorVisit doctorVisit)
        {
            try
            {
                await _contextApi.DoctorVisits.AddAsync(doctorVisit);
                await _contextApi.SaveChangesAsync();
                return new OkResponse();
            }
            catch(Exception exception)
            {
                return new DatabaseExceptionResponse(exception);
            }
        }

        public async Task<OneOf<DoctorVisit?, DatabaseExceptionResponse>> GetDoctorVisitAsync(int doctorVisitId)
        {
            try
            {
                DoctorVisit? doctorVisit = await _contextApi.DoctorVisits.SingleOrDefaultAsync(x => x.Id == doctorVisitId);
                return doctorVisit;
            }
            catch(Exception exception)
            {
                return new DatabaseExceptionResponse(exception);
            }
        }

        public async Task<OneOf<List<DoctorVisit>, DatabaseExceptionResponse>> GetDoctorVisitListAsync(int doctorId)
        {
            try
            {
                List<DoctorVisit> doctorVisitsList = await _contextApi.DoctorVisits
                    .Include(x => x.Calendar)
                    .Where(x => x.Calendar.DoctorId == doctorId)
                    .ToListAsync();

                return doctorVisitsList;
            }
            catch(Exception exception)
            {
                return new DatabaseExceptionResponse(exception);
            }
        }
    }
}
