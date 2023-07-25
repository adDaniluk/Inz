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

        public DoctorRepository(DbContextApi dbContextApi)
        {
            _dbContextApi = dbContextApi;
        }

        public async Task<OneOf<OkResponse, DatabaseExceptionResponse>> InsertDoctorAsync(Doctor doctor)
        {
            try
            {
                await _dbContextApi.AddAsync(doctor);
                await _dbContextApi.SaveChangesAsync();
                return new OkResponse();
            }
            catch (Exception exception)
            {
                return new DatabaseExceptionResponse(exception);
            }
        }

        public async Task<OneOf<OkResponse, DatabaseExceptionResponse>> UpdateDoctorAsync(Doctor doctor)
        {
            try
            {
                _dbContextApi.Doctors.Update(doctor);
                await _dbContextApi.SaveChangesAsync();
                return new OkResponse();
            }catch(Exception exception)
            {
                return new DatabaseExceptionResponse(exception);
            }
        }
        public async Task<OneOf<OkResponse, NotFoundResponse, DatabaseExceptionResponse>> AddDoctorServiceAsync(ServiceDoctorDTO serviceDTO)
        {
            //try
            //{
            //    string log;
            //    var doesServiceExist = await _dbContextApi.Services.FirstOrDefaultAsync(x => x.Id == serviceDTO.ServiceId);
            //    var doesDoctorExist = await _dbContextApi.Doctors.FirstOrDefaultAsync(x => x.Id == serviceDTO.DoctorId);

            //    if (doesDoctorExist != null && doesServiceExist != null)
            //    {
            //        DoctorServices doctorService = new DoctorServices()
            //        {
            //            Doctor = doesDoctorExist,
            //            Service = doesServiceExist,
            //            Price = serviceDTO.Price
            //        };

            //        await _dbContextApi.DoctorServices.AddAsync(doctorService);
            //        await _dbContextApi.SaveChangesAsync();
            //    }

            //    if (doesServiceExist == null || doesDoctorExist == null)
            //    {
            //        log = "";
            //        //log = !doesServiceExist ?
            //        //    $"Service with Id: {serviceDTO.ServiceId} does not exist" :
            //        //    $"Doctor with Id: {serviceDTO.DoctorId} does not exist";

            //        _logger.LogError(message: log);
            //        return new NotFoundResponse(log);
            //    }

            //    log = "Service has been added with success";
            //    _logger.LogInformation(message: log);
            //    return new OkResponse(log);
            //}
            //catch(Exception execption)
            //{
            //    _logger.LogError(message: $"{execption.Message}");
            //    return new DatabaseExceptionResponse(execption);
            //}
            return new OkResponse();
        }

        public async Task<OneOf<OkResponse, NotFoundResponse, DatabaseExceptionResponse>> RemoveDoctorServiceAsync(ServiceDoctorDTO serviceDTO)
        {
            //string log;
            //try
            //{
            //    bool doesServiceExist = await _dbContextApi.Services.AnyAsync(x => x.Id == serviceDTO.ServiceId);
            //    bool doesDoctorExist = await _dbContextApi.Doctors.AnyAsync(x => x.Id == serviceDTO.DoctorId);

            //    if (doesDoctorExist && doesServiceExist)
            //    {
            //        bool doesServiceExistInCalendar = await _dbContextApi.Calendars.AnyAsync(x => x.ServiceId == serviceDTO.ServiceId && x.DoctorId == serviceDTO.DoctorId);

            //        if (!doesServiceExistInCalendar)
            //        {
            //            var serviceDoctorToRemove = await _dbContextApi.DoctorServices.FirstAsync(x => x.ServiceId == serviceDTO.ServiceId && x.DoctorId == serviceDTO.DoctorId);
            //            _dbContextApi.DoctorServices.Remove(serviceDoctorToRemove);
            //            await _dbContextApi.SaveChangesAsync();
            //        }
            //        log = $"Service cannot be removed - it is used in doctor visit.";
            //        _logger.LogInformation(message: log);
            //        return new OkResponse(log);
            //    }

            //    if (!doesServiceExist || !doesDoctorExist)
            //    {
            //        log = !doesServiceExist ?
            //        $"Service with Id: {serviceDTO.ServiceId} does not exist" :
            //        $"Doctor with Id: {serviceDTO.DoctorId} does not exist";

            //        _logger.LogError(message: log);
            //        return new NotFoundResponse(log);
            //    }

            //    log = $"Service: {serviceDTO.ServiceId} has been added to Doctor: {serviceDTO.DoctorId} with price {serviceDTO.Price}";
            //    _logger.LogInformation(message: log);
            //    return new OkResponse(log);

            //}
            //catch (Exception execption)
            //{
            //    _logger.LogError(message: $"{execption.Message}");
            //    return new DatabaseExceptionResponse(execption);
            //}
            return new OkResponse();
        }

        public async Task<OneOf<Doctor, NotFoundResponse, DatabaseExceptionResponse>> GetDoctorAsync(int id)
        {
            try
            {
                Doctor? doctor = await _dbContextApi.Doctors
                    .Include(x => x.Address)
                    .Include(x => x.MedicalSpecializations)
                    .Include(x => x.DoctorServices)
                    .SingleOrDefaultAsync(x => x.Id == id && x.IsDeleted == 0);

                return doctor != null ? doctor : new NotFoundResponse();
            }
            catch(Exception exception)
            {
                return new DatabaseExceptionResponse(exception);
            }
        }
    }
}
