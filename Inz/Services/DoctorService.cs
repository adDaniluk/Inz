using Inz.DTOModel;
using Inz.Model;
using Inz.OneOfHelper;
using Inz.Repository;
using OneOf;
using OneOf.Types;

namespace Inz.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;

        public DoctorService(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public async Task<OneOf<Doctor, DatabaseException>> InsertDoctorAsync(DoctorDTO doctorDTO)
        {

            Doctor doctor = new Doctor()
            {
                Login = doctorDTO.Login,
                Password = doctorDTO.Password,
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
                MedicalSpecializations = doctorDTO.MedicalSpecializations
            };

            await _doctorRepository.InsertDoctorAsync(doctor);
            OneOf<Doctor,DatabaseException> returnValue = await _doctorRepository.SaveChangesAsync();

            return returnValue.Match(
                doctor => new Doctor(),
                databaseExcption => returnValue);
        }

        public async Task<OneOf<Doctor, NotFound, DatabaseException>> UpdateDoctorAsync(UpdateDoctorDTO updateDoctorDTO)
        {
            var returnValue = await _doctorRepository.UpdateDoctorAsync(updateDoctorDTO);

            returnValue.Match(
                doctor => doctor,
                notFound => new NotFound(),
                databaseException => returnValue);

            return returnValue;
        }
    }
}
