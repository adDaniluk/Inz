using FluentValidation.Results;
using Inz.DTOModel;
using Inz.DTOModel.Validators;
using Inz.Model;
using Inz.OneOfHelper;
using Inz.Repository;
using OneOf;
using System.Data.Common;

namespace Inz.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMedicalSpecializationRepository _medicalSpecializationRepository; 
        private readonly ILogger _logger;

        public DoctorService(IDoctorRepository doctorRepository,
            IMedicalSpecializationRepository medicalSpecializationRepository, 
            ILogger<IDoctorService> logger)
        {
            _doctorRepository = doctorRepository;
            _medicalSpecializationRepository = medicalSpecializationRepository;
            _logger = logger;
        }

        public async Task<OneOf<OkResponse, NotValidateResponse, DatabaseExceptionResponse>> InsertDoctorAsync(DoctorDTO doctorDTO)
        {
            string log;
            var validateResult = DoctorDTOValidation(doctorDTO);

            if(!validateResult.IsValid)
            {
                log = "DoctorDTO is not valid";
                _logger.LogInformation(message: log);
                return new NotValidateResponse(validateResult);
            }

            Doctor doctor = new Doctor()
            {
                Login = doctorDTO.Login,
                Password = doctorDTO.Password,
                UserId = doctorDTO.UserId,
                Email = doctorDTO.Email,
                Phone = doctorDTO.Phone,
                Name = doctorDTO.Name,
                Surname = doctorDTO.Surname,
                DateOfBirth = doctorDTO.DateOfBirth,
                Timestamp = DateTime.Now,
                AlterTimestamp = DateTime.Now,
                Address = new Address()
                {
                    Street = doctorDTO.Street,
                    City = doctorDTO.City,
                    PostCode = doctorDTO.PostCode,
                    AparmentNumber = doctorDTO.AparmentNumber
                },
            };

            var callbackInsertDoctor = await _doctorRepository.InsertDoctorAsync(doctor);

            if(callbackInsertDoctor.TryPickT0(out OkResponse okResponse, out var dbException))
            {
                log = "Doctor has been created";
                _logger.LogInformation(message: log);
                okResponse.ResponseMessage = log;
                return okResponse;
            }

            log = $"Error on a database, see inner exception: {dbException.Exception.Message}";
            _logger.LogError(message: log);
            return dbException;
        }

        public async Task<OneOf<OkResponse, NotFoundResponse, NotValidateResponse, DatabaseExceptionResponse>> UpdateDoctorAsync(UpdateDoctorDTO updateDoctorDTO)
        {
            OneOf<OkResponse, NotFoundResponse, NotValidateResponse, DatabaseExceptionResponse> responseHandler = new ();
            string log;
            var validateResult = UpdateDoctorDTOValidation(updateDoctorDTO);

            if (!validateResult.IsValid)
            {
                log = "UpdateDoctorDTO is not valid";
                _logger.LogError(message: log);
                return new NotValidateResponse(validateResult);
            }

            var callbackDoctorToUpdate = await _doctorRepository.GetDoctorAsync(updateDoctorDTO.Id);

            if (callbackDoctorToUpdate.TryPickT0(out Doctor doctorToUpdate, out var remainErrors))
            {
                if (updateDoctorDTO.MedicalSpecializationsId.Any())
                {
                    var medicalSpecializationsToUpdate = await _medicalSpecializationRepository.GetMedicalSpecializationAsync(updateDoctorDTO.MedicalSpecializationsId.ToList());

                    medicalSpecializationsToUpdate.Switch(
                        list =>
                        {
                            doctorToUpdate.MedicalSpecializations = list;
                        },
                        notFound =>
                        {
                            log = "Provided medical specialization(s) does not exist";
                            _logger.LogInformation(message: log);
                            responseHandler = new NotFoundResponse(log);
                        },
                        dbException =>
                        {
                            log = $"Database exception, please look into: {dbException.Exception}";
                            _logger.LogError(message: log);
                            responseHandler = dbException;
                        });

                    if (!medicalSpecializationsToUpdate.IsT0)
                    {
                        return responseHandler;
                    }
                }


                doctorToUpdate.Email = updateDoctorDTO.Email;
                doctorToUpdate.Phone = updateDoctorDTO.Phone;
                doctorToUpdate.Address.Street = updateDoctorDTO.Street;
                doctorToUpdate.Address.City = updateDoctorDTO.City;
                doctorToUpdate.Address.PostCode = updateDoctorDTO.PostCode;
                doctorToUpdate.Address.AparmentNumber = updateDoctorDTO.AparmentNumber;
                doctorToUpdate.AlterTimestamp = DateTime.Now;
                doctorToUpdate.Biography = updateDoctorDTO.Biography;

                var callbackUpdateDoctor = await _doctorRepository.UpdateDoctorAsync(doctorToUpdate);

                if (callbackUpdateDoctor.TryPickT0(out OkResponse okResponse, out DatabaseExceptionResponse dbErrorResponse))
                {
                    log = $"Doctor with ID: {updateDoctorDTO.Id} has been updated";
                    _logger.LogInformation(log);
                    return new OkResponse(log);
                }

                return dbErrorResponse;
            }

            remainErrors.Switch(
                    notFound =>
                    {
                        log = $"Doctor with id {updateDoctorDTO.Id} does not exist.";
                        _logger.LogError(message: log);
                        responseHandler = new NotFoundResponse(log);
                    },
                    dbException =>
                    {
                        log = $"Database exception, please look into: {dbException.Exception}";
                        _logger.LogError(message: log);
                    });

            return responseHandler;
        }

        public async Task<OneOf<OkResponse, NotFoundResponse, DatabaseExceptionResponse>> AddDoctorServiceAsync(ServiceDoctorDTO serviceDTO)
        {
            //var returnValue = await _doctorRepository.AddDoctorServiceAsync(serviceDTO);

            //return returnValue.Match(
            //    okResponse => okResponse,
            //    notFound => notFound,
            //    databaseException => returnValue);
            return new OkResponse();
        }

        public async Task<OneOf<OkResponse, NotFoundResponse, DatabaseExceptionResponse>> RemoveDoctorServiceAsync(ServiceDoctorDTO serviceDTO)
        {
            var returnValue = await _doctorRepository.RemoveDoctorServiceAsync(serviceDTO);

            return new OkResponse();
        }

        private ValidationResult DoctorDTOValidation(DoctorDTO doctorDTO)
        {
            DotorDTOValidator doctorDTOValidation = new DotorDTOValidator();
            var validatorResult = doctorDTOValidation.Validate(doctorDTO);

            return validatorResult;
        }

        private ValidationResult UpdateDoctorDTOValidation(UpdateDoctorDTO updateDoctorDTO)
        {
            UpdateDoctorDTOValidator updateDoctorDTOValidator = new UpdateDoctorDTOValidator();
            var validatorResult = updateDoctorDTOValidator.Validate(updateDoctorDTO);

            return validatorResult;
        }
    }
}
