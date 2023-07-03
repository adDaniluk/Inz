using Inz.Context;
using Inz.DTOModel;
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

        public async Task InsertPatientAsync(Patient patient)
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

        public async Task<OneOf<Patient, NotFound, DatabaseException>> UpdatePatientAsyc(UpdatePatientDTO updatePatientDTO)
        {
            try
            {
                var patientToUpdate = await _dbContextApi.Patients.Include(x => x.Address).SingleOrDefaultAsync(x => x.Id == updatePatientDTO.Id);

                if (patientToUpdate == null)
                {
                    return new NotFound();
                }

                patientToUpdate.Email = updatePatientDTO.Email;
                patientToUpdate.Phone = updatePatientDTO.Phone;
                patientToUpdate.Address.Street = updatePatientDTO.Street;
                patientToUpdate.Address.City = updatePatientDTO.City;
                patientToUpdate.Address.PostCode = updatePatientDTO.PostCode;
                patientToUpdate.Address.AparmentNumber = updatePatientDTO.AparmentNumber;
                patientToUpdate.AlterTimestamp = DateTime.Now;
                
                await _dbContextApi.SaveChangesAsync();

                return patientToUpdate;

            }catch(Exception exception)
            {
                return new DatabaseException(exception);
            }
        }
    }
}
