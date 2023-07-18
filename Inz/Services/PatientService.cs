using Inz.DTOModel;
using Inz.Model;
using Inz.OneOfHelper;
using Inz.Repository;
using OneOf;

namespace Inz.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;

        public PatientService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<OneOf<OkResponse, DatabaseExceptionResponse>> InsertPatientAsync(PatientDTO patientDTO)
        {

            Patient patient = new Patient()
            {
                Login = patientDTO.Login,
                Password = patientDTO.Password,
                UserId = patientDTO.UserId,
                Email = patientDTO.Email,
                Phone = patientDTO.Phone,
                Name = patientDTO.Name,
                Surname = patientDTO.Surname,
                DateOfBirth = patientDTO.DateOfBirth.Date,
                Timestamp = DateTime.Now,
                AlterTimestamp = DateTime.Now,
                Address = new Address()
                {
                    Street = patientDTO.Street,
                    City = patientDTO.City,
                    PostCode = patientDTO.PostCode,
                    AparmentNumber = patientDTO.AparmentNumber
                }
        };

            await _patientRepository.InsertPatientAsync(patient);
            OneOf<OkResponse, DatabaseExceptionResponse> returnValue = await _patientRepository.SaveChangesAsync();

            return returnValue.Match(
                okResponse => okResponse,
                databaseException => returnValue);
        }

        public async Task<OneOf<OkResponse, NotFoundResponse, DatabaseExceptionResponse>> UpdatePatientAsyc(UpdatePatientDTO updatePatientDTO)
        {
            var returnValue = await _patientRepository.UpdatePatientAsyc(updatePatientDTO);

            return returnValue.Match(
                okResponse => okResponse,
                notFound => notFound,
                databaseException => returnValue);
        }
    }
}
