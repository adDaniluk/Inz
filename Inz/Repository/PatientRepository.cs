using Inz.Context;
using Inz.Model;

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

        public async Task SaveChangesAsync()
        {
            await _dbContextApi.SaveChangesAsync();
        }
    }
}
