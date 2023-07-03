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

        public DoctorRepository(DbContextApi dbContextApi)
        {
            _dbContextApi = dbContextApi;
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
                return new DatabaseException(exception);
            }

            return new Doctor();
        }

        public async Task<OneOf<Doctor, NotFound, DatabaseException>> UpdateDoctorAsync(UpdateDoctorDTO updateDoctorDTO)
        {
            try
            {
                var doctorToUpdate = await _dbContextApi.Doctors.Include(x => x.Address).Include(x => x.MedicalSpecializations)
                    .SingleOrDefaultAsync(x => x.Id == updateDoctorDTO.Id && x.MedicalSpecializations == updateDoctorDTO.MedicalSpecializations);

                if (doctorToUpdate == null)
                {
                    return new NotFound();
                }

                doctorToUpdate.Email = updateDoctorDTO.Email;
                doctorToUpdate.Phone = updateDoctorDTO.Phone;
                doctorToUpdate.Address.Street = updateDoctorDTO.Street;
                doctorToUpdate.Address.City = updateDoctorDTO.City;
                doctorToUpdate.Address.PostCode = updateDoctorDTO.PostCode;
                doctorToUpdate.Address.AparmentNumber = updateDoctorDTO.AparmentNumber;
                doctorToUpdate.AlterTimestamp = DateTime.Now;
                doctorToUpdate.Biography = updateDoctorDTO.Biography;
                doctorToUpdate.MedicalSpecializations = updateDoctorDTO.MedicalSpecializations;
                

                await _dbContextApi.SaveChangesAsync();

                return doctorToUpdate;


            }catch(Exception e)
            {
                return new DatabaseException(e);
            }
        }
    }
}
