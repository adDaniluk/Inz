using Inz.Context;
using Inz.Model;
using Inz.OneOfHelper;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace Inz.Repository
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly DbContextApi _dbContextApi;

        public DoctorRepository(DbContextApi dbContextApi)
        {
            _dbContextApi = dbContextApi;
        }

        public async Task<OneOf<OkResponse, DatabaseExceptionResponse>> InsertDoctorAsync(Doctor doctor)
        {
            try
            {
                await _dbContextApi.AddAsync(doctor);
                await _dbContextApi.SaveChangesAsync();
                return new OkResponse();
            }
            catch (Exception exception)
            {
                return new DatabaseExceptionResponse(exception);
            }
        }

        public async Task<OneOf<OkResponse, DatabaseExceptionResponse>> UpdateDoctorAsync(Doctor doctor)
        {
            try
            {
                _dbContextApi.Doctors.Update(doctor);
                await _dbContextApi.SaveChangesAsync();
                return new OkResponse();
            }catch(Exception exception)
            {
                return new DatabaseExceptionResponse(exception);
            }
        }

        public async Task<OneOf<Doctor, NotFoundResponse, DatabaseExceptionResponse>> GetDoctorAsync(int id)
        {
            try
            {
                Doctor? doctor = await _dbContextApi.Doctors
                    .Include(x => x.Address)
                    .Include(x => x.MedicalSpecializations)
                    .Include(x => x.DoctorServices)
                    .SingleOrDefaultAsync(x => x.Id == id && x.IsDeleted == 0);

                return doctor != null ? doctor : new NotFoundResponse();
            }
            catch(Exception exception)
            {
                return new DatabaseExceptionResponse(exception);
            }
        }
    }
}
