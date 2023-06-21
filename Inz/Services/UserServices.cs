using Inz.Context;
using Inz.DTOModel;
using Inz.Model;
using Inz.Repository;

namespace Inz.Services
{
    public class UserServices : IUserServices
    {
        private readonly PatientRepository _patientRepository;
        public async Task RegisterUser(PatientDTO patientDTO, AddressDTO addressDTO)
        {
            Address address = new Address()
            {
                Street = addressDTO.Street,
                City = addressDTO.City,
                PostCode = addressDTO.PostCode,
                AparmentNumber = addressDTO.AparmentNumber,
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
                DateOfBirth = patientDTO.DateOfBirth
            };

            await _patientRepository.InserNewPatient(patient, address);
        }

        public async Task ValidateUser()
        {
            Task t = Task.Delay(1);
            await t;
        }
    }
}
