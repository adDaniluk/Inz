using Inz.Context;
using Inz.Model;
using Inz.OneOfHelper;
using OneOf;
using OneOf.Types;

namespace Inz.Repository
{
    public class PatientRepository : IPatientRepository
    {
        private readonly DbContextApi _dbContextApi;

        public PatientRepository(DbContextApi dbContext)
        {
            _dbContextApi = dbContext;
        }

        public async Task InsertNewPatientAsync(Patient patient)
        {
            await _dbContextApi.Patients.AddAsync(patient);
        }

        public async Task<OneOf<Patient, DatabaseException>> SaveChangesAsync()
        {
            try
            {
                await _dbContextApi.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return new DatabaseException(e);
            }
            return new Patient();
        }

        public async Task<OneOf<bool, DatabaseException>> CheckIfPatientExistAsync(int id)
        {
            try
            {
                var patientCheck = await _dbContextApi.Patients.FindAsync(id);

                if (patientCheck != null)
                {
                    return true;
                }

            }catch(Exception e)
            {
                return new DatabaseException(e);
            }

            return false;
        }

        public async Task UpdatePatientAsync(Patient patient)
        {
           // update patient
        }
    }
}
