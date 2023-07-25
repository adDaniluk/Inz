using FluentValidation.Results;
using Inz.DTOModel;
using Inz.DTOModel.Validators;
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

        public PatientService(IPatientRepository patientRepository, ILogger<IPatientService> logger)
        {
            _patientRepository = patientRepository;
            _logger = logger;
        }

        public async Task<OneOf<OkResponse, NotValidateResponse, DatabaseExceptionResponse>> InsertPatientAsync(PatientDTO patientDTO)
        {
            string log;
            var validateResult = PatientDTOValidation(patientDTO);

            if (!validateResult.IsValid)
            {
                log = "PatientDTO is not valid.";
                _logger.LogInformation(message: log);
                return new NotValidateResponse(validateResult);
            }

            Patient patient = new Patient()
            {
                Login = patientDTO.Login,
                Password = patientDTO.Password,
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

            if (callbackInsertPatient.TryPickT0(out OkResponse okResponse, out DatabaseExceptionResponse dbException))
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

        public async Task<OneOf<OkResponse, NotFoundResponse, NotValidateResponse, DatabaseExceptionResponse>> UpdatePatientAsyc(UpdatePatientDTO updatePatientDTO)
        {
            string log;
            var validateResult = UpdatePatientDTOValidation(updatePatientDTO);

            if (!validateResult.IsValid)
            {     
                _logger.LogInformation(message: "PatientDTO is not valid.");
                return new NotValidateResponse(validateResult);
            }

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

                _logger.LogError(message: exceptionOnUpdate.Exception.Message);
                return exceptionOnUpdate;
            }

            _logger.LogError(message: exceptionResponse.Exception.Message);
            return exceptionResponse;
        }

        private static ValidationResult PatientDTOValidation(PatientDTO patientDTO)
        {
            PatientDTOValidator patientDTOvalidator = new PatientDTOValidator();
            var validatorResult = patientDTOvalidator.Validate(patientDTO);

            return validatorResult;
        }

        private static ValidationResult UpdatePatientDTOValidation(UpdatePatientDTO updatePatientDTO)
        {
            UpdatePatientDTOValidator updatePatientDTOValidator = new UpdatePatientDTOValidator();
            var validatorResult = updatePatientDTOValidator.Validate(updatePatientDTO);

            return validatorResult;
        }
    } }
