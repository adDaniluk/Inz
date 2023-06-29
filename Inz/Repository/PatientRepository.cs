using Inz.Context;
using Inz.Model;
using Inz.OneOfHelper;
using Microsoft.EntityFrameworkCore;
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

        public async Task<OneOf<Patient, NotFound, DatabaseException>> UpdatePatientAsync(Patient patient)
        {
            try
            {
                var patientToUpdate = await _dbContextApi.Patients.Include(x => x.Address).SingleOrDefaultAsync(x => x.PatientId == patient.PatientId);

                if (patientToUpdate == null)
                {
                    return new NotFound();
                }

                //TODO: mapowanie z DTO do pacjenta 
                //TODO: zmiana na wszystkich encji Id
                //patientToUpdate.Surname = patient.Surname;

                //_dbContextApi.Attach(patient);
                //_dbContextApi.Entry(patient).Property(x => x.Email).IsModified = true;
                //_dbContextApi.SaveChanges();

                //await _dbContextApi.Patients.Update(patient);

                return patient;

            }catch(Exception exception)
            {
                return new DatabaseException(exception);
            }
        }
    }
}
