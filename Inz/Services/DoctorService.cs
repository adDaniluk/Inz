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

        public DoctorService(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public async Task<OneOf<Doctor, DatabaseException>> InsertNewDoctorAsync(DoctorDTO doctorDTO)
        {
            Address address = new Address()
            {
                Street = doctorDTO.Street,
                City = doctorDTO.City,
                PostCode = doctorDTO.PostCode,
                AparmentNumber = doctorDTO.AparmentNumber,
                Timestamp = DateTime.Now,
                AlterTimestamp = DateTime.Now
            };

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
                AlterTimestamp = DateTime.Now
            };

            await _doctorRepository.InsertNewDoctorAsync(doctor);
            OneOf<Doctor,DatabaseException> returnValue = await _doctorRepository.SaveChangesAsync();

            return returnValue.Match(
                doctor => new Doctor(),
                databaseExcption => returnValue);
        }
    }
}
