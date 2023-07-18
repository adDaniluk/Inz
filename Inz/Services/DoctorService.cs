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
        public async Task<OneOf<OkResponse, DatabaseExceptionResponse>> InsertDoctorAsync(DoctorDTO doctorDTO)
        {

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
                //TODO MedicalSpecializations map
                //MedicalSpecializations = doctorDTO.MedicalSpecializations
            };

            await _doctorRepository.InsertDoctorAsync(doctor);
            OneOf<OkResponse,DatabaseExceptionResponse> returnValue = await _doctorRepository.SaveChangesAsync();

            return returnValue.Match(
                okResponse => okResponse,
                databaseExcption => returnValue);
        }

        public async Task<OneOf<OkResponse, NotFoundResponse, DatabaseExceptionResponse>> UpdateDoctorAsync(UpdateDoctorDTO updateDoctorDTO)
        {
            var returnValue = await _doctorRepository.UpdateDoctorAsync(updateDoctorDTO);

            return returnValue.Match(
                okResponse => okResponse,
                notFound => notFound,
                databaseException => returnValue);
        }

        public async Task<OneOf<OkResponse, NotFoundResponse, DatabaseExceptionResponse>> AddDoctorServiceAsync(ServiceDoctorDTO serviceDTO)
        {
            var returnValue = await _doctorRepository.AddDoctorServiceAsync(serviceDTO);

            return returnValue.Match(
                okResponse => okResponse,
                notFound => notFound,
                databaseException => returnValue);
        }

        public async Task<OneOf<OkResponse, NotFoundResponse, DatabaseExceptionResponse>> RemoveDoctorServiceAsync(ServiceDoctorDTO serviceDTO)
        {
            var returnValue = await _doctorRepository.RemoveDoctorServiceAsync(serviceDTO);

            return returnValue.Match(
                okResponse => okResponse,
                notFound => notFound,
                databaseException => returnValue);
        }
    }
}
