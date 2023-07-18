using Inz.Context;
using Inz.DTOModel;
using Inz.Model;
using Inz.OneOfHelper;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace Inz.Repository
{
    public class PatientRepository : IPatientRepository
    {
        private readonly DbContextApi _dbContextApi;
        private readonly ILogger _logger;

        public PatientRepository(DbContextApi dbContext, ILogger<IPatientRepository> logger)
        {
            _dbContextApi = dbContext;
            _logger = logger;

        }

        public async Task InsertPatientAsync(Patient patient)
        {
            await _dbContextApi.Patients.AddAsync(patient);
        }

        public async Task<OneOf<OkResponse, DatabaseExceptionResponse>> SaveChangesAsync()
        {
            try
            {
                await _dbContextApi.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                _logger.LogError(message: exception.Message);
                return new DatabaseExceptionResponse(exception);
            }

            string log = "Patient has been created.";
            _logger.LogInformation(message: log);
            return new OkResponse(log);
        }

        public async Task<OneOf<OkResponse, NotFoundResponse, DatabaseExceptionResponse>> UpdatePatientAsyc(UpdatePatientDTO updatePatientDTO)
        {
            try
            {
                string log;
                var patientToUpdate = await _dbContextApi.Patients.Include(x => x.Address).SingleOrDefaultAsync(x => x.Id == updatePatientDTO.Id);

                if (patientToUpdate == null)
                {
                    log = $"Patient with id: {updatePatientDTO.Id} does not exist.";
                    _logger.LogInformation(message: log);
                    return new NotFoundResponse(log);
                }

                patientToUpdate.Email = updatePatientDTO.Email;
                patientToUpdate.Phone = updatePatientDTO.Phone;
                patientToUpdate.Address.Street = updatePatientDTO.Street;
                patientToUpdate.Address.City = updatePatientDTO.City;
                patientToUpdate.Address.PostCode = updatePatientDTO.PostCode;
                patientToUpdate.Address.AparmentNumber = updatePatientDTO.AparmentNumber;
                patientToUpdate.AlterTimestamp = DateTime.Now;
                
                await _dbContextApi.SaveChangesAsync();

                log = "Patient has been updated with success.";
                _logger.LogInformation(message: log);
                return new OkResponse(log);

            }catch(Exception exception)
            {
                _logger.LogError(message: exception.Message);
                return new DatabaseExceptionResponse(exception);
            }
        }
    }
}
