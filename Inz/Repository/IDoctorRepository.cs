using Inz.Model;
using Inz.OneOfHelper;
using OneOf;

namespace Inz.Repository
{
    public interface IDoctorRepository
    {
        public Task InsertNewDoctorAsync(Doctor doctor);
        public Task<OneOf<Doctor, DatabaseException>> SaveChangesAsync();
    }
}
