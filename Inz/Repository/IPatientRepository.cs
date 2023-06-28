using Inz.Model;
using Inz.OneOfHelper;
using OneOf;

namespace Inz.Repository
{
    public interface IPatientRepository
    {
        public Task InsertNewPatientAsync(Patient patient);
        public Task<OneOf<Patient, DatabaseException>> SaveChangesAsync();
        public Task<OneOf<bool, DatabaseException>> CheckIfPatientExistAsync(int id);
        public Task UpdatePatientAsync(Patient patient);
    }
}
