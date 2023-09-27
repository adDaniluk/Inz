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
            }
            catch(Exception exception)
            {
                return new DatabaseExceptionResponse(exception);
            }
        }

        public async Task<OneOf<Doctor?, DatabaseExceptionResponse>> GetDoctorByIdAsync(int id)
        {
            try
            {
                Doctor? doctor = await _dbContextApi.Doctors
                    .Include(x => x.Address)
                    .Include(x => x.MedicalSpecializations)
                    .Include(x => x.DoctorServices)
                    .Include(x => x.Diseases)
                    .SingleOrDefaultAsync(x => x.Id == id && x.IsDeleted == 0);

                return doctor;
            }
            catch(Exception exception)
            {
                return new DatabaseExceptionResponse(exception);
            }
        }


        public async Task<OneOf<bool, DatabaseExceptionResponse>> CheckExistingLoginAsync(string login)
        {
            try
            {
                Doctor? doctor = await _dbContextApi.Doctors.SingleOrDefaultAsync(x => x.Login.ToLower() == login.ToLower());
                return doctor == null;
            }
            catch(Exception exception)
            {
                return new DatabaseExceptionResponse(exception);
            }
        }

        public async Task<OneOf<string, DatabaseExceptionResponse>> GetPasswordAsync(string login)
        {
            try
            {
                Doctor? doctor = await _dbContextApi.Doctors.SingleOrDefaultAsync(x => x.Login == login);
                return doctor == null ? "" : doctor.Password;

            }
            catch(Exception exception)
            {
                return new DatabaseExceptionResponse(exception);
            }
        }
    }
}
