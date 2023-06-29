using Inz.DTOModel;
using Inz.Model;
using Inz.OneOfHelper;
using Inz.Repository;
using OneOf;
using OneOf.Types;
using System.Reflection.Metadata.Ecma335;

namespace Inz.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;

        public PatientService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<OneOf<Patient, DatabaseException>> InsertPatientAsync(PatientDTO patientDTO)
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
            OneOf<Patient, DatabaseException> returnValue = await _patientRepository.SaveChangesAsync();

            return returnValue.Match(
                patinet => new Patient(),
                databaseException => returnValue);
        }

        public async Task<OneOf<Patient, NotFound, DatabaseException>> UpdatePatientAsync(UpdatePatientDTO updatePatientDTO, int id)
        {
            
            var checkPatient = await _patientRepository.CheckIfPatientExistAsync(id);


            //if(checkPatient.Value is true)
            //{
            //    await _patientRepository.UpdatePatientAsync(new Patient());
            //    await _patientRepository.SaveChangesAsync();
            //    return new Patient();
            //}

            //if(checkPatient.Value is false)
            //{
            //    return new NotFound();
            //}

            //return checkPatient.AsT1;

            checkPatient.Match(
                bool1 => bool1 ? 
                ,
                databaseException => checkPatient);
        }

        private async Task<OneOf<Patient, DatabaseException>> ValidateAndUpdatePatientAsyc(UpdatePatientDTO updatePatientDTO)
        {
            // getPatientById -> model patient <=> updatePatientDto -> 

            await _patientRepository.UpdatePatientAsync(new Patient()); //updatePatientDTO
            await _patientRepository.SaveChangesAsync();
            return new Patient();
        }

    }
}
