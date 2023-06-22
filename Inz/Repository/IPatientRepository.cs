using Inz.Model;

namespace Inz.Repository
{
    public interface IPatientRepository
    {
        public Task InsertNewPatientAsync(Patient patient);
        public Task SaveChangesAsync();
    }
}
