using Inz.Context;
using Inz.DTOModel;
using Inz.Model;
using Inz.OneOfHelper;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;

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

        public async Task<OneOf<Doctor, DatabaseException>> SaveChangesAsync()
        {
            try
            {
                await _dbContextApi.SaveChangesAsync();
            }
            catch(Exception exception)
            {
                _logger.LogError(message: exception.Message);
                return new DatabaseException(exception);
            }

            return new Doctor();
        }

        public async Task<OneOf<Doctor, NotFound, DatabaseException>> UpdateDoctorAsync(UpdateDoctorDTO updateDoctorDTO)
        {
            try
            {
                var doctorToUpdate = await _dbContextApi.Doctors.Include(x => x.Address).Include(x => x.MedicalSpecializations)
                    .SingleOrDefaultAsync(x => x.Id == updateDoctorDTO.Id);

                if (doctorToUpdate == null)
                {
                    _logger.LogError(message: $"Doctor with Id:{updateDoctorDTO.Id} does not exist");
                    return new NotFound();
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

                return doctorToUpdate;

            }catch(Exception exception)
            {
                _logger.LogError(message: $"Error: {exception}");
                return new DatabaseException(exception);
            }
        }
        public async Task<OneOf<DoctorServices, NotFound, DatabaseException>> AddDoctorServiceAsync(ServiceDoctorDTO serviceDTO)
        {
            try
            {
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
                
                if (!doesServiceExist)
                {
                    _logger.LogError(message: $"Service with Id: {serviceDTO.ServiceId} does not exist");
                    return new NotFound();
                }
                else if (!doesDoctorExist)
                {
                    _logger.LogError(message: $"Doctor with Id: {serviceDTO.DoctorId} does not exist");
                    return new NotFound();
                }

                return new DoctorServices();

            }catch(Exception execption)
            {
                _logger.LogError(message: $"{execption.Message}");
                return new DatabaseException(execption);
            }
        }
    }
}
