using Inz.DTOModel;
using Inz.Model;
using Inz.OneOfHelper;
using Inz.Repository;
using OneOf;
using System.Data.Common;

namespace Inz.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IPasswordHashService _passwordHashService;
        private readonly ILogger _logger;

        public PatientService(IPatientRepository patientRepository,
            ILogger<IPatientService> logger,
            IPasswordHashService passwordHashService)
        {
            _patientRepository = patientRepository;
            _passwordHashService = passwordHashService;
            _logger = logger;
        }

        public async Task<OneOf<OkResponse, AlreadyExistResponse, DatabaseExceptionResponse>> InsertPatientAsync(PatientDTO patientDTO)
        {
            string log;

            var callbackCheckLoginAvailability = await _patientRepository.CheckExistingLoginAsync(patientDTO.Login);

            if (callbackCheckLoginAvailability.TryPickT1(out var dbErrorLoginCheck, out var loginAvailibilityCheck))
            {
                log = $"Error on a database, see inner exception: {dbErrorLoginCheck.Exception.Message}";
                _logger.LogError(message: log);
                return dbErrorLoginCheck;
            }

            if(!loginAvailibilityCheck)
            {
                log = $"Login with a name {patientDTO.Login} is already taken, please insert a new one - has to be uniq";
                _logger.LogError(message: log);
                return new AlreadyExistResponse(log);
            }

            Patient patient = new Patient()
            {
                Login = patientDTO.Login,
                Password = _passwordHashService.GetHash(patientDTO.Password),
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
                _logger.LogInformation(message: log);
                okResponse.ResponseMessage = log;
                return okResponse;
            }

            log = $"Error on a database, see inner exception: {dbException.Exception.Message}";
            _logger.LogError(message: log);
            return dbException;
        }

        public async Task<OneOf<OkResponse, NotFoundResponse, DatabaseExceptionResponse>> UpdatePatientAsyc(UpdatePatientDTO updatePatientDTO)
        {
            string log;

            var callbackPatientToUpdate = await _patientRepository.GetPatientAsync(updatePatientDTO.Id);

            if (callbackPatientToUpdate.TryPickT1(out NotFoundResponse notFound, out var oneOfPatientOrDatabaseException))
            {
                log = $"Patient with id: {updatePatientDTO.Id} does not exist.";
                _logger.LogInformation(message: log);
                notFound.ResponseMessage = log;
                return notFound;
            }

            if (oneOfPatientOrDatabaseException.TryPickT0(out Patient patient, out var exceptionResponse))
            {
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
                    _logger.LogInformation(message: log);
                    okResponse.ResponseMessage = log;
                    return okResponse;
                }

                log = $"Error on a database, see inner exception: {exceptionOnUpdate.Exception.Message}";
                _logger.LogError(message: log);
                return exceptionOnUpdate;
            }

            log = $"Error on a database, see inner exception: {exceptionResponse.Exception.Message}";
            _logger.LogError(message: log);
            return exceptionResponse;
        }
    }
}
