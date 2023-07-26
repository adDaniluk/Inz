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
        private readonly IServiceRepository _serviceRepository;
        private readonly IDoctorServiceRepository _doctorServiceRepository;
        private readonly ILogger _logger;

        public DoctorService(IDoctorRepository doctorRepository,
            IMedicalSpecializationRepository medicalSpecializationRepository, 
            IServiceRepository serviceRepository,
            IDoctorServiceRepository doctorServiceRepository,
            ILogger<IDoctorService> logger)
        {
            _doctorRepository = doctorRepository;
            _medicalSpecializationRepository = medicalSpecializationRepository;
            _serviceRepository = serviceRepository;
            _doctorServiceRepository = doctorServiceRepository;
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

            Doctor doctor = new()
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

            if(callbackInsertDoctor.TryPickT0(out OkResponse okResponse, out DatabaseExceptionResponse dbException))
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

        public async Task<OneOf<OkResponse, NotFoundResponse, NotValidateResponse, DatabaseExceptionResponse>> AddDoctorServiceAsync(ServiceDoctorDTO serviceDoctorDTO)
        {

            string log;
            var validateResult = AddDoctorServiceDTOValidation(serviceDoctorDTO);

            if(!validateResult.IsValid)
            {
                log = "DerviceDoctorDTO is not valid";
                _logger.LogError(message: log);
                return new NotValidateResponse(validateResult);
            }

            var callbackDoctor = _doctorRepository.GetDoctorAsync(serviceDoctorDTO.DoctorId);
            var callbackService = _serviceRepository.GetServiceAsync(serviceDoctorDTO.ServiceId);
            var callbackDoctorService = _doctorServiceRepository.GetDoctorServiceAsync(serviceDoctorDTO.DoctorId, serviceDoctorDTO.ServiceId);

            Task.WaitAll(callbackDoctor, callbackService, callbackDoctorService);

            if(callbackDoctor.Result.IsT2 || callbackService.Result.IsT2 || callbackDoctorService.Result.IsT2)
            {
                DatabaseExceptionResponse dbException;
                dbException = callbackDoctor.Result.IsT2 ? callbackDoctor.Result.AsT2 : (callbackService.Result.IsT2 ? callbackService.Result.AsT2 : callbackDoctorService.Result.AsT2);
                log = $"Database exception, please look into: {dbException.Exception}";
                _logger.LogError(message: log);
                return dbException;
            }

            if(callbackDoctor.Result.IsT1 || callbackService.Result.IsT1 || callbackDoctorService.Result.IsT0)
            {
                if (callbackDoctor.Result.IsT1)
                {
                    log = $"Doctor with id {serviceDoctorDTO.DoctorId} does not exist";
                    _logger.LogInformation(message: log);
                }
                else if(callbackService.Result.IsT1)
                {
                    log = $"Service with id {serviceDoctorDTO.ServiceId} does not exist";
                    _logger.LogInformation(message: log);
                }
                else
                {
                    log = $"DoctorService with service id: {serviceDoctorDTO.ServiceId} and doctor id: {serviceDoctorDTO.DoctorId} already exist.";
                    _logger.LogInformation(message: log);
                }

                return new NotFoundResponse(log);
            }

            Doctor doctor = callbackDoctor.Result.AsT0;
            Service service = callbackService.Result.AsT0;

            DoctorServices doctorService = new()
            {
                Service = service,
                Doctor = doctor,
                Price = serviceDoctorDTO.Price
            };

            var callbackAddDoctorService = await _doctorServiceRepository.AddDoctorServiceAsync(doctorService);

            if(callbackAddDoctorService.IsT0)
            {
                log = $"DoctorService has been added";
                _logger.LogInformation(message: log);
                return new OkResponse(log);
            }

            return new DatabaseExceptionResponse(callbackAddDoctorService.AsT1.Exception);
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

        private ValidationResult AddDoctorServiceDTOValidation(ServiceDoctorDTO serviceDoctorDTO)
        {
            ServiceDoctorDTOValidator serviceDoctorDTOValidator = new ServiceDoctorDTOValidator();
            var validatorResult = serviceDoctorDTOValidator.Validate(serviceDoctorDTO);

            return validatorResult;
        }
    }
}
