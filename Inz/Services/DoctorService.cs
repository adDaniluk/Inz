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
        private readonly IDiseaseRepository _diseaseRepository;
        private readonly ILogger _logger;
        
        

        public DoctorService(IDoctorRepository doctorRepository,
            IMedicalSpecializationRepository medicalSpecializationRepository, 
            IServiceRepository serviceRepository,
            IDoctorServiceRepository doctorServiceRepository,
            IDiseaseRepository diseaseRepository,
            ILogger<IDoctorService> logger)
        {
            _doctorRepository = doctorRepository;
            _medicalSpecializationRepository = medicalSpecializationRepository;
            _serviceRepository = serviceRepository;
            _doctorServiceRepository = doctorServiceRepository;
            _diseaseRepository = diseaseRepository;
            _logger = logger;
        }

        public async Task<OneOf<OkResponse, AlreadyExistResponse, DatabaseExceptionResponse>> InsertDoctorAsync(DoctorDTO doctorDTO)
        {
            string log;

            var callbackCheckLoginAvailability = await _doctorRepository.CheckExistingLoginAsync(doctorDTO.Login);

            if(callbackCheckLoginAvailability.TryPickT1(out var databaseException, out var loginAvailibilityCheck))
            {
                log = $"Error on a database, see inner exception: {databaseException.Exception.Message}";
                _logger.LogError(message: log);
                return databaseException;
            }

            if (!loginAvailibilityCheck)
            {
                log = $"Login with a name {doctorDTO.Login} is already taken, please insert a new one - has to be uniq";
                _logger.LogError(message: log);
                return new AlreadyExistResponse(log);
            }

            Doctor doctor = new()
            {
                Login = doctorDTO.Login,
                Password = PasswordHashService.GetHash(doctorDTO.Password),
                UserId = doctorDTO.UserId,
                Email = doctorDTO.Email,
                Phone = doctorDTO.Phone,
                Name = doctorDTO.Name,
                Surname = doctorDTO.Surname,
                DateOfBirth = doctorDTO.DateOfBirth,
                Timestamp = DateTime.Now,
                AlterTimestamp = DateTime.Now,
                LicenseNumber = doctorDTO.LicenseNumber,
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
            OneOf<OkResponse, NotFoundResponse, DatabaseExceptionResponse> responseHandler = new();
            string log;

            var callbackDoctorToUpdate = await _doctorRepository.GetDoctorByIdAsync(updateDoctorDTO.Id);

            if (callbackDoctorToUpdate.TryPickT0(out Doctor? doctorToUpdate, out var dbError))
            {
                if(doctorToUpdate == null)
                {
                    log = $"Doctor with id {updateDoctorDTO.Id} does not exist.";
                    _logger.LogError(message: log);
                    responseHandler = new NotFoundResponse(log);
                    return responseHandler;
                }

                if (updateDoctorDTO.MedicalSpecializationsId.Any())
                {
                    var medicalSpecializationsToUpdate = await _medicalSpecializationRepository.GetMedicalSpecializationAsync(updateDoctorDTO.MedicalSpecializationsId.ToList());

                    medicalSpecializationsToUpdate.Switch(
                        medicalSpecializationsList =>
                        {
                            if (medicalSpecializationsList.Any())
                            {
                                doctorToUpdate.MedicalSpecializations = medicalSpecializationsList;
                            }
                        },
                        dbException =>
                        {
                            log = $"Database exception, please look into: {dbException.Exception.Message}";
                            _logger.LogError(message: log);
                            responseHandler = dbException;
                        });

                    if (medicalSpecializationsToUpdate.IsT1)
                    {
                        return responseHandler;
                    }
                }

                if(updateDoctorDTO.CuredDiseasesId.Any())
                {
                    var curedDiseasesToUpdate = await _diseaseRepository.GetDiseaseAsync(updateDoctorDTO.CuredDiseasesId.ToList());

                    curedDiseasesToUpdate.Switch(
                        curedDiseasesList => {

                            if (curedDiseasesList.Any())
                            {
                                doctorToUpdate.Diseases = curedDiseasesList;
                            }
                        },
                    dbException =>
                    {
                        log = $"Database exception, please look into: {dbException.Exception.Message}";
                        _logger.LogError(message: log);
                        responseHandler = dbException;
                    });

                    if (curedDiseasesToUpdate.IsT1)
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

                callbackUpdateDoctor.Switch(
                    okResponse =>
                    {
                        log = $"Doctor with ID: {updateDoctorDTO.Id} has been updated";
                        _logger.LogInformation(message: log);
                        responseHandler = new OkResponse(log);
                    },
                    dbErrorUpdateDoctor =>
                    {
                        log = $"Database exception, please look into: {dbError.Exception.Message}";
                        _logger.LogError(message: log);
                        responseHandler = dbErrorUpdateDoctor;
                    });

                return responseHandler;
            }

            log = $"Database exception, please look into: {dbError.Exception.Message}";
            _logger.LogError(message: log);
            return dbError;
        }

        public async Task<OneOf<OkResponse, NotFoundResponse, DatabaseExceptionResponse>> AddDoctorServiceAsync(DoctorServiceDTO serviceDoctorDTO)
        {
            string log;

            var callbackDoctor = await _doctorRepository.GetDoctorByIdAsync(serviceDoctorDTO.DoctorId);
            var callbackService = await _serviceRepository.GetServiceAsync(serviceDoctorDTO.ServiceId);
            var callbackDoctorService = await _doctorServiceRepository.GetDoctorServiceAsync(serviceDoctorDTO.DoctorId, serviceDoctorDTO.ServiceId);

            //await Task.WhenAll(callbackDoctor, callbackService, callbackDoctorService);

            callbackDoctor.TryPickT0(out var doctor, out var dbErrorDoctor);
            callbackService.TryPickT0(out var service, out var dbErrorService);
            callbackDoctorService.TryPickT0(out var doctorServices, out var dbErrorDoctorService);

            if(dbErrorDoctor != null || dbErrorService != null || dbErrorDoctorService != null)
            {
                DatabaseExceptionResponse dbException = dbErrorDoctor != null ? dbErrorDoctor : (dbErrorService != null ? dbErrorService : dbErrorDoctorService);
                log = $"Database exception, please look into: {dbException.Exception.Message}";
                _logger.LogError(message: log);
                return dbException;
            }

            if (doctor == null)
            {
                log = $"Doctor with id {serviceDoctorDTO.DoctorId} does not exist";
                _logger.LogInformation(message: log);
                return new NotFoundResponse(log);
            }

            if(service == null)
            {
                log = $"Service with id {serviceDoctorDTO.ServiceId} does not exist";
                _logger.LogInformation(message: log);
                return new NotFoundResponse(log);
            }

            if(doctorServices != null)
            {
                log = $"DoctorService with service id: {serviceDoctorDTO.ServiceId} and doctor id: {serviceDoctorDTO.DoctorId} already exist.";
                _logger.LogInformation(message: log);
                return new NotFoundResponse(log);
            }

            DoctorServices doctorService = new()
            {
                ServiceId = serviceDoctorDTO.ServiceId,
                DoctorId = serviceDoctorDTO.DoctorId,
                Price = serviceDoctorDTO.Price
            };

            var callbackAddDoctorService = await _doctorServiceRepository.AddDoctorServiceAsync(doctorService);

            if(callbackAddDoctorService.TryPickT0(out var okResponse, out var dbErrorAddDoctorService))
            {
                log = $"DoctorService has been added";
                _logger.LogInformation(message: log);
                okResponse.ResponseMessage = log;
                return okResponse;
            }

            log = $"Database exception, please look into: {dbErrorAddDoctorService.Exception.Message}";
            _logger.LogError(message: log);
            return dbErrorAddDoctorService;
        }

        public async Task<OneOf<OkResponse, NotFoundResponse, DatabaseExceptionResponse>> RemoveDoctorServiceAsync(RemoveDoctorServiceDTO removeDoctorServiceDTO)
        {
            string log;

            var callbackDoctorService = await _doctorServiceRepository.GetDoctorServiceAsync(removeDoctorServiceDTO.DoctorId, removeDoctorServiceDTO.ServiceId);

            callbackDoctorService.TryPickT0(out var doctorServices, out var remainderErrors);
            
            if(doctorServices != null)
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

                log = $"Database exception, please look into: {dbError.Exception.Message}";
                _logger.LogError(message: log);
                return dbError;
            }

            return remainderErrors;
            //if (remainderErrors.IsT0)
            //{
            //    log = $"DoctorService does not exist.";
            //    _logger.LogInformation(message: log);
            //    return new NotFoundResponse(log);
            //}
            //else
            //{
            //    log = $"Database exception, please look into: {remainderErrors.AsT1.Exception.Message}";
            //    _logger.LogError(message: log);
            //    return remainderErrors.AsT1;
            //}
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
