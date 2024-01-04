using Inz.Context;
using Inz.Model;
using Inz.OneOfHelper;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace Inz.Repository
{
    public class PatientRepository : IPatientRepository
    {
        private readonly DbContextApi _dbContextApi;

        public PatientRepository(DbContextApi dbContext)
        {
            _dbContextApi = dbContext;
        }

        public async Task<OneOf<Patient, NotFoundResponse, DatabaseExceptionResponse>> GetPatientAsync(int id)
        {
            try
            {
                Patient? patient = await _dbContextApi.Patients.Include(x => x.Address).FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == 0);
                return patient != null ? patient : new NotFoundResponse();
            }
            catch (Exception exception)
            {
                return new DatabaseExceptionResponse(exception);
            }
        }

        public async Task<OneOf<OkResponse, DatabaseExceptionResponse>> InsertPatientAsync(Patient patient)
        {
            try
            {
                await _dbContextApi.Patients.AddAsync(patient);
                await _dbContextApi.SaveChangesAsync();
                return new OkResponse();
            }
            catch(Exception exception)
            {
                return new DatabaseExceptionResponse(exception);
            }
        }

        public async Task<OneOf<OkResponse, DatabaseExceptionResponse>> UpdatePatientAsyc(Patient patient)
        {
            try
            {
                _dbContextApi.Patients.Update(patient);
                await _dbContextApi.SaveChangesAsync();
                return new OkResponse();
            }
            catch(Exception exception)
            {
                return new DatabaseExceptionResponse(exception);
            }
        }
        //TODO: Switch checkExisting Login with GetPatientByLogin
        public async Task<OneOf<bool, DatabaseExceptionResponse>> CheckExistingLoginAsync(string login)
        {
            try
            {
                Patient? patient = await _dbContextApi.Patients.SingleOrDefaultAsync(x => x.Login.ToLower() == login.ToLower());
                return patient == null;
            }
            catch(Exception exception)
            {
                return new DatabaseExceptionResponse(exception);
            }
        }

        public async Task<OneOf<Patient?, DatabaseExceptionResponse>> GetPatientByLoginAsync(string login)
        {
            try
            {
                Patient? patient = await _dbContextApi.Patients.SingleOrDefaultAsync(x => x.Login.ToLower() == login.ToLower());
                return patient;

            }
            catch (Exception exception)
            {
                return new DatabaseExceptionResponse(exception);
            }
        }
    }
}
