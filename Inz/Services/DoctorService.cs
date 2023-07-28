using Inz.DTOModel;
using Inz.Model;
using Inz.OneOfHelper;
using Inz.Repository;
using OneOf;

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

        public async Task<OneOf<OkResponse, DatabaseExceptionResponse>> InsertDoctorAsync(DoctorDTO doctorDTO)
        {
            string log;

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

        public async Task<OneOf<OkResponse, NotFoundResponse, DatabaseExceptionResponse>> UpdateDoctorAsync(UpdateDoctorDTO updateDoctorDTO)
        {
            OneOf<OkResponse, NotFoundResponse, DatabaseExceptionResponse> responseHandler = new ();
            string log;

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

        public async Task<OneOf<OkResponse, NotFoundResponse, DatabaseExceptionResponse>> AddDoctorServiceAsync(DoctorServiceDTO serviceDoctorDTO)
        {

            string log;

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

            DoctorServices doctorService = new()
            {
                ServiceId = serviceDoctorDTO.ServiceId,
                DoctorId = serviceDoctorDTO.DoctorId,
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

        public async Task<OneOf<OkResponse, NotFoundResponse, DatabaseExceptionResponse>> RemoveDoctorServiceAsync(RemoveDoctorServiceDTO removeDoctorServiceDTO)
        {
            string log;

            var callbackDoctorService = await _doctorServiceRepository.GetDoctorServiceAsync(removeDoctorServiceDTO.DoctorId, removeDoctorServiceDTO.ServiceId);

            if(callbackDoctorService.TryPickT0(out DoctorServices doctorServices, out var remainderErrors))
            {
                DoctorServices doctorServiceToRemove = doctorServices;

                var callbackDeleteServiceDoctor = await _doctorServiceRepository.RemoveDoctorServiceAsync(doctorServiceToRemove);
               
                if(callbackDeleteServiceDoctor.TryPickT0(out OkResponse okResponse, out var dbError))
                {
                    log = $"DoctorService has been removed successfuly.";
                    _logger.LogInformation(message: log);
                    okResponse.ResponseMessage = log;
                    return okResponse;
                }

                log = $"Database exception, please look into: {dbError.Exception}";
                _logger.LogError(message: log);
                return dbError;
            }

            if (remainderErrors.IsT0)
            {
                log = $"DoctorService does not exist.";
                _logger.LogInformation(message: log);
                return new NotFoundResponse(log);
            }
            else
            {
                log = $"Database exception, please look into: {remainderErrors.AsT1.Exception}";
                _logger.LogError(message: log);
                return remainderErrors.AsT1;
            }
        }

        //Generic validation method -> as a sample
        /*
        private ValidationResult DTOValidation<T, V>(T valueDTO) where V : AbstractValidator<T>, new() where T : IDTOModelValidator, new()
        {
            V DTOValidator = new();
            var validatorResult = DTOValidator.Validate(valueDTO);
            return validatorResult;
        }
        */
    }
}
