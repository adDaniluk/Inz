using Inz.Model;
using Inz.OneOfHelper;
using Inz.Repository;
using OneOf;

namespace Inz.Services
{
    public class PasswordHashService : IPasswordHashService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly ILogger _logger;

        public PasswordHashService(IPatientRepository patientRepository,
            IDoctorRepository doctorRepository,
            ILogger<IPasswordHashService> logger)
        {
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
            _logger = logger;
        }

        public string GetHash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public async Task<OneOf<bool, DatabaseExceptionResponse>> ValidatePassword(string password, string login, PersonType personType)
        {
            string log;
            OneOf<string, DatabaseExceptionResponse> callbackPassword = new();

            switch (personType)
            {
                case PersonType.Doctor:
                    {
                        callbackPassword = await _doctorRepository.GetPasswordAsync(login);
                    }
                    break;
                case PersonType.Patient:
                    {
                        callbackPassword = await _patientRepository.GetPasswordAsync(login);
                    }
                    break;
            }

            if(callbackPassword.TryPickT1(out var dbError, out string hashPassword))
            {
                log = $"Database exception, please look into: {dbError.Exception.Message}";
                _logger.LogError(message: log);
                return dbError;
            }

            return BCrypt.Net.BCrypt.Verify(password, hashPassword);
        }
    }
}
