using Inz.Context;
using Inz.DTOModel;
using Inz.Model;

namespace Inz.Repository
{
    public class PatientRepository
    {
        private readonly DbContextApi _dbContextApi1;
        public PatientRepository(DbContextApi dbContextApi)
        {
            this._dbContextApi1 = dbContextApi;
        }

        public async Task InserNewPatient(Patient patient, Address address)
        {

            await _dbContextApi1.Addresses.AddAsync(address);

            var addressId = address.Id;

            patient.AddressId = addressId;

            await _dbContextApi1.Patients.AddAsync(patient);

            _dbContextApi1.SaveChanges();
        }
    }
}
