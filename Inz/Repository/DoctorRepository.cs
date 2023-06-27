using Inz.Context;
using Inz.Model;
using Inz.OneOfHelper;
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

        public async Task InsertNewDoctorAsync(Doctor doctor)
        {
            await _dbContextApi.AddAsync(doctor);
        }

        public async Task<OneOf<Doctor, DatabaseException>> SaveChangesAsync()
        {
            try
            {
                await _dbContextApi.SaveChangesAsync();
            }
            catch(Exception exception)
            {
                return new DatabaseException(exception);
            }

            return new Doctor();
        }
    }
}
