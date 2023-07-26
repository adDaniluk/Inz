using Inz.Context;
using Inz.Model;
using Inz.OneOfHelper;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace Inz.Repository
{
    public class DoctorServiceRepository : IDoctorServiceRepository
    {
        private readonly DbContextApi _dbContextApi;
        public DoctorServiceRepository(DbContextApi dbContextApi)
        {
            _dbContextApi = dbContextApi;
        }

        public async Task<OneOf<DoctorServices, NotFoundResponse, DatabaseExceptionResponse>> GetDoctorServiceAsync(int doctorId, int serviceId)
        {
            try
            {
                DoctorServices? doctorServices = await _dbContextApi.DoctorServices.SingleOrDefaultAsync(x => x.DoctorId == doctorId && x.ServiceId == serviceId);

                return doctorServices != null ? doctorServices : new NotFoundResponse();
            }
            catch (Exception exception)
            {
                return new DatabaseExceptionResponse(exception);
            }
        }

        public async Task<OneOf<OkResponse, DatabaseExceptionResponse>> AddDoctorServiceAsync(DoctorServices doctorService)
        {
            try
            {
                await _dbContextApi.DoctorServices.AddAsync(doctorService);
                await _dbContextApi.SaveChangesAsync();
                return new OkResponse();
            }
            catch(Exception exception)
            {
                return new DatabaseExceptionResponse(exception);
            }
        }

        public Task<OneOf<IList<DoctorServices>, NotFoundResponse, DatabaseExceptionResponse>> GetDoctorServiceByDoctorIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
