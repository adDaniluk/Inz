using Inz.Context;
using Inz.Model;
using Inz.OneOfHelper;
using Microsoft.Data.SqlClient;
using OneOf;

namespace Inz.Repository
{
    public class PatientRepository: IPatientRepository
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

        public async Task<OneOf<Patient, DisconnectFromDatabase>> SaveChangesAsync()
        {
            if (_dbContextApi.Database.CanConnect())
            {
                await _dbContextApi.SaveChangesAsync();
                return new Patient();
            }
            return new DisconnectFromDatabase();
        }
        

    }
}
