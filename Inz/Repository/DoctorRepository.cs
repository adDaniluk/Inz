using Inz.Context;
using Inz.DTOModel;
using Inz.Model;
using Inz.OneOfHelper;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace Inz.Repository
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly DbContextApi _dbContextApi;
        private readonly ILogger _logger;

        public DoctorRepository(DbContextApi dbContextApi, ILogger<IDoctorRepository> logger)
        {
            _dbContextApi = dbContextApi;
            _logger = logger;
        }

        public async Task InsertDoctorAsync(Doctor doctor)
        {
            await _dbContextApi.AddAsync(doctor);
        }

        public async Task<OneOf<OkResponse, DatabaseExceptionResponse>> SaveChangesAsync()
        {
            try
            {
                await _dbContextApi.SaveChangesAsync();
            }
            catch(Exception exception)
            {
                _logger.LogError(message: exception.Message);
                return new DatabaseExceptionResponse(exception);
            }
            string log = "Doctor has been created";
            _logger.LogInformation(message: log);
            return new OkResponse(log);
        }

        public async Task<OneOf<OkResponse, NotFoundResponse, DatabaseExceptionResponse>> UpdateDoctorAsync(UpdateDoctorDTO updateDoctorDTO)
        {
            try
            {
                string log;
                var doctorToUpdate = await _dbContextApi.Doctors.Include(x => x.Address).Include(x => x.MedicalSpecializations)
                    .SingleOrDefaultAsync(x => x.Id == updateDoctorDTO.Id);

                if (doctorToUpdate == null)
                {
                    log = $"Doctor with Id:{updateDoctorDTO.Id} does not exist";
                    _logger.LogError(message: log);
                    return new NotFoundResponse(log);
                }

                if (updateDoctorDTO.MedicalSpecializationId != null)
                {
                    var medicalSpecializationsToUpdate = await _dbContextApi.MedicalSpecializations.Where(x => updateDoctorDTO.MedicalSpecializationId.Contains(x.Id)).ToListAsync();
                    doctorToUpdate.MedicalSpecializations = medicalSpecializationsToUpdate;
                }

                doctorToUpdate.Email = updateDoctorDTO.Email;
                doctorToUpdate.Phone = updateDoctorDTO.Phone;
                doctorToUpdate.Address.Street = updateDoctorDTO.Street;
                doctorToUpdate.Address.City = updateDoctorDTO.City;
                doctorToUpdate.Address.PostCode = updateDoctorDTO.PostCode;
                doctorToUpdate.Address.AparmentNumber = updateDoctorDTO.AparmentNumber;
                doctorToUpdate.AlterTimestamp = DateTime.Now;
                doctorToUpdate.Biography = updateDoctorDTO.Biography;

                await _dbContextApi.SaveChangesAsync();

                log = $"Doctor has been updated";
                _logger.LogInformation(message: log);
                return new OkResponse(log);

            }catch(Exception exception)
            {
                _logger.LogError(message: $"Error: {exception}");
                return new DatabaseExceptionResponse(exception);
            }
        }
        public async Task<OneOf<OkResponse, NotFoundResponse, DatabaseExceptionResponse>> AddDoctorServiceAsync(ServiceDoctorDTO serviceDTO)
        {
            try
            {
                string log;
                bool doesServiceExist = await _dbContextApi.Services.AnyAsync(x => x.Id == serviceDTO.ServiceId);
                bool doesDoctorExist = await _dbContextApi.Doctors.AnyAsync(x => x.Id == serviceDTO.DoctorId);

                if (doesDoctorExist && doesServiceExist)
                {
                    DoctorServices doctorService = new DoctorServices()
                    {
                        DoctorId = serviceDTO.DoctorId,
                        ServiceId = serviceDTO.ServiceId,
                        Price = serviceDTO.Price
                    };

                    await _dbContextApi.DoctorServices.AddAsync(doctorService);
                    await _dbContextApi.SaveChangesAsync();
                }
                
                if (!doesServiceExist || !doesDoctorExist)
                {

                    log = !doesServiceExist ?
                        $"Service with Id: {serviceDTO.ServiceId} does not exist" :
                        $"Doctor with Id: {serviceDTO.DoctorId} does not exist";
                    
                    _logger.LogError(message: log);
                    return new NotFoundResponse(log);
                }

                log = "Service has been added with success";
                _logger.LogInformation(message: log);
                return new OkResponse(log);
            }
            catch(Exception execption)
            {
                _logger.LogError(message: $"{execption.Message}");
                return new DatabaseExceptionResponse(execption);
            }
        }

        public async Task<OneOf<OkResponse, NotFoundResponse, DatabaseExceptionResponse>> RemoveDoctorServiceAsync(ServiceDoctorDTO serviceDTO)
        {
            string log;
            try
            {
                bool doesServiceExist = await _dbContextApi.Services.AnyAsync(x => x.Id == serviceDTO.ServiceId);
                bool doesDoctorExist = await _dbContextApi.Doctors.AnyAsync(x => x.Id == serviceDTO.DoctorId);

                if (doesDoctorExist && doesServiceExist)
                {
                    bool doesServiceExistInCalendar = await _dbContextApi.Calendars.AnyAsync(x => x.ServiceId == serviceDTO.ServiceId && x.DoctorId == serviceDTO.DoctorId);

                    if (!doesServiceExistInCalendar)
                    {
                        var serviceDoctorToRemove = await _dbContextApi.DoctorServices.FirstAsync(x => x.ServiceId == serviceDTO.ServiceId && x.DoctorId == serviceDTO.DoctorId);
                        _dbContextApi.DoctorServices.Remove(serviceDoctorToRemove);
                        await _dbContextApi.SaveChangesAsync();
                    }
                    log = $"Service cannot be removed - it is used in doctor visit.";
                    _logger.LogInformation(message: log);
                    return new OkResponse(log);
                }

                if (!doesServiceExist || !doesDoctorExist)
                {
                    log = !doesServiceExist ?
                    $"Service with Id: {serviceDTO.ServiceId} does not exist" :
                    $"Doctor with Id: {serviceDTO.DoctorId} does not exist";

                    _logger.LogError(message: log);
                    return new NotFoundResponse(log);
                }

                log = $"Service: {serviceDTO.ServiceId} has been added to Doctor: {serviceDTO.DoctorId} with price {serviceDTO.Price}";
                _logger.LogInformation(message: log);
                return new OkResponse(log);

            }
            catch (Exception execption)
            {
                _logger.LogError(message: $"{execption.Message}");
                return new DatabaseExceptionResponse(execption);
            }
        }
    }
}
