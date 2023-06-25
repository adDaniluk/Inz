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

        public async Task<OneOf<Patient, DisconnectFromDatabase>> InsertPatientAsync(PatientDTO patientDTO)
        {
            Address address = new Address()
            {
                Street = patientDTO.Street,
                City = patientDTO.City,
                PostCode = patientDTO.PostCode,
                AparmentNumber = patientDTO.AparmentNumber,
                Timestamp = DateTime.Now,
                AlterTimestamp = DateTime.Now
            };

            Patient patient = new Patient()
            {
                Login = patientDTO.Login,
                Password = patientDTO.Password,
                Email = patientDTO.Email,
                Phone = patientDTO.Phone,
                Name = patientDTO.Name,
                Surname = patientDTO.Surname,
                DateOfBirth = patientDTO.DateOfBirth.Date,
                Timestamp = DateTime.Now,
                AlterTimestamp = DateTime.Now,
                Address = address
            };

            await _patientRepository.InsertNewPatientAsync(patient);
            var returnValue = await _patientRepository.SaveChangesAsync();

            if(returnValue.Value.GetType() == typeof(DisconnectFromDatabase))
            {
                return new DisconnectFromDatabase();
            }

            return new Patient();
        }

        public async Task ValidateUser()
        {
            Task t = Task.Delay(1);
            await t;
        }
    }
}
