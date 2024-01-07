using Inz.DTOModel;
using Inz.Helpers;
using Inz.Model;
using Inz.OneOfHelper;
using Inz.Repository;
using OneOf;

namespace Inz.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly ILogger _logger;

        public PatientService(IPatientRepository patientRepository,
            ILogger<IPatientService> logger)
        {
            _patientRepository = patientRepository;
            _logger = logger;
        }

        public async Task<OneOf<Patient, NotFoundResponse, DatabaseExceptionResponse>> GetPatientProfileAsync(int id)
        {
            string log;

            var callbackPatient = await _patientRepository.GetPatientByIdAsync(id);

            if(callbackPatient.TryPickT0(out var patient, out var databaseException))
            {
                if(patient !=  null)
                {
                    log = $"Patient profile has been sent with Id: {id}";
                    _logger.LogInformation("{log}", log);
                    return patient;
                }

                log = $"Patient with Id: {id} does not exist.";
                _logger.LogInformation("{log}", log);
                return new NotFoundResponse(log);
            }

            log = $"Error on a database, see inner exception: {databaseException.Exception.Message}";
            _logger.LogError("{log}", log);
            return databaseException;
        }

        public async Task<OneOf<OkResponse, AlreadyExistResponse, DatabaseExceptionResponse>> InsertPatientAsync(PatientDTO patientDTO)
        {
            string log;

            var callbackCheckLoginAvailability = await _patientRepository.GetPatientByLoginAsync(patientDTO.Login);

            if (callbackCheckLoginAvailability.TryPickT1(out var dbErrorLoginCheck, out var loginAvailibilityCheck))
            {
                log = $"Error on a database, see inner exception: {dbErrorLoginCheck.Exception.Message}";
                _logger.LogError("{log}", log);
                return dbErrorLoginCheck;
            }

            if (loginAvailibilityCheck != null)
            {
                log = $"Login with a name {patientDTO.Login} is already taken, please insert a new one - has to be uniq";
                _logger.LogError("{log}", log);
                return new AlreadyExistResponse(log);
            }

            Patient patient = new()
            {
                Login = patientDTO.Login,
                Password = PasswordHashHelper.GetHash(patientDTO.Password),
                UserId = patientDTO.UserId,
                Email = patientDTO.Email,
                Phone = patientDTO.Phone,
                Name = patientDTO.Name,
                Surname = patientDTO.Surname,
                DateOfBirth = patientDTO.DateOfBirth.Date,
                Timestamp = DateTime.Now,
                AlterTimestamp = DateTime.Now,
                Address = new Address()
                {
                    Street = patientDTO.Street,
                    City = patientDTO.City,
                    PostCode = patientDTO.PostCode,
                    AparmentNumber = patientDTO.AparmentNumber
                }
            };

            var callbackInsertPatient = await _patientRepository.InsertPatientAsync(patient);

            if (callbackInsertPatient.TryPickT0(out var okResponse, out var dbException))
            {
                log = "Patient has been added";
                _logger.LogInformation("{log}", log);
                okResponse.ResponseMessage = log;
                return okResponse;
            }

            log = $"Error on a database, see inner exception: {dbException.Exception.Message}";
            _logger.LogError("{log}", log);
            return dbException;
        }

        public async Task<OneOf<OkResponse, NotFoundResponse, DatabaseExceptionResponse>> UpdatePatientAsyc(UpdatePatientDTO updatePatientDTO)
        {
            string log;

            var callbackPatientToUpdate = await _patientRepository.GetPatientByIdAsync(updatePatientDTO.Id);

            if (callbackPatientToUpdate.TryPickT0(out Patient? patient, out var databaseException))
            {
                if(patient == null)
                {
                    log = $"Patient with id: {updatePatientDTO.Id} does not exist.";
                    _logger.LogInformation("{log}", log);
                    return new NotFoundResponse(log);
                }

                patient.Email = updatePatientDTO.Email;
                patient.Phone = updatePatientDTO.Phone;
                patient.Address.Street = updatePatientDTO.Street;
                patient.Address.City = updatePatientDTO.City;
                patient.Address.PostCode = updatePatientDTO.PostCode;
                patient.Address.AparmentNumber = updatePatientDTO.AparmentNumber;
                patient.AlterTimestamp = DateTime.Now;

                var callbackUpdatePatient = await _patientRepository.UpdatePatientAsyc(patient);

                if (callbackUpdatePatient.TryPickT0(out OkResponse okResponse, out var exceptionOnUpdate))
                {
                    log = $"Patient with ID: {patient.Id} has been updated";
                    _logger.LogInformation("{log}", log);
                    okResponse.ResponseMessage = log;
                    return okResponse;
                }

                log = $"Error on a database, see inner exception: {exceptionOnUpdate.Exception.Message}";
                _logger.LogError("{log}", log);
                return exceptionOnUpdate;
            }

            log = $"Error on a database, see inner exception: {databaseException.Exception.Message}";
            _logger.LogError("{log}", log);
            return databaseException;
        }
    }
}
