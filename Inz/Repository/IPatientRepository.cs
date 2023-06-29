using Inz.Model;
using Inz.OneOfHelper;
using OneOf;
using OneOf.Types;

namespace Inz.Repository
{
    public interface IPatientRepository
    {
        public Task InsertNewPatientAsync(Patient patient);
        public Task<OneOf<Patient, DatabaseException>> SaveChangesAsync();
        public Task<OneOf<bool, DatabaseException>> CheckIfPatientExistAsync(int id);
        public Task<OneOf<Patient, NotFound, DatabaseException>> UpdatePatientAsync(Patient patient);
    }
}
