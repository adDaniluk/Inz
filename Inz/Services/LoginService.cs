using Inz.DTOModel;
using Inz.Model;
using Inz.OneOfHelper;
using Microsoft.IdentityModel.Tokens;
using OneOf;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Inz.Repository;

namespace Inz.Services
{
    public class LoginService : ILoginService
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IPatientRepository _patientRepository;

        public LoginService(ILogger<ILoginService> logger,
            IPatientRepository patientRepository, 
            IDoctorRepository doctorRepository,
            IConfiguration configuration)
        {
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<OneOf<OkResponse, NotAutorizedResponse, DatabaseExceptionResponse>> SignIn(LoginDTO loginDTO)
        {
            string log;
            bool isValidated = false;
            int userId = new ();

            switch (loginDTO.PersonType)
            {
                case PersonType.Doctor:
                    {
                        var callbackDoctor = await _doctorRepository.GetDoctorByLoginAsync(loginDTO.Login);

                        if(callbackDoctor.TryPickT1(out var databaseError, out Doctor? doctor))
                        {
                            log = $"Error on a database, see inner exception: {databaseError.Exception.Message}";
                            _logger.LogError(message: log);
                            return databaseError;
                        }

                        if (doctor != null && PasswordHashService.ValidatePassword(loginDTO.Password, doctor.Password))
                        {
                            userId = doctor.Id;
                            isValidated = true;
                            break;
                        }
                        break;
                    }
                case PersonType.Patient:
                    {
                        var callbackPatient =  await _patientRepository.GetPatientByLoginAsync(loginDTO.Login);

                        if(callbackPatient.TryPickT1(out var databaseError, out Patient? patient))
                        {
                            log = $"Error on a database, see inner exception: {databaseError.Exception.Message}";
                            _logger.LogError(message: log);
                            return databaseError;
                        }

                        if (patient != null && PasswordHashService.ValidatePassword(loginDTO.Password, patient.Password))
                        {
                            userId = patient.Id;
                            isValidated = true;
                            break;
                        }
                        break;
                    }
            }


            if(!isValidated)
            {
                log = "Username password does not match!";
                _logger.LogInformation(message: log);
                return new NotAutorizedResponse(log);
            }

            var jwtToken = CreateToken(loginDTO, userId);

            if(jwtToken.TryPickT0(out string token, out var notAutorized))
            {
                log = $"User: {loginDTO.Login} has logged in";
                _logger.LogInformation(message: log);
                return new OkResponse(token);
            }

            log = "JWT could not be created";
            _logger.LogInformation(message: log);
            notAutorized.ReponseMessage = log;
            return notAutorized;
        }

        private OneOf<string, NotAutorizedResponse> CreateToken(LoginDTO loginDTO, int userId)
        {
            List<Claim> claims = new();
            OneOf<string, NotAutorizedResponse> jwtHandler;
            string log;

            string signingKey = _configuration.GetSection("Token")["ServiceApiKey"]!;
            string audience = _configuration.GetSection("Token")["Audience"]!;
            string issuer = _configuration.GetSection("Token")["Issuer"]!;
            int expireTimeInMinutesJWT = Int16.Parse(_configuration.GetSection("Token")["ExpireTimeInMinutesJWT"]!);
            string role;

            role = loginDTO.PersonType == PersonType.Patient ? "Patient" : "Doctor";

            claims.Add(new Claim(ClaimTypes.Role, role));
            claims.Add(new Claim("Id", userId.ToString()));

            if(signingKey == null)
            {
                log = "SigningKey configuration is missing";
                _logger.LogInformation(message: log);
            }

            if(audience == null)
            {
                log = "Audience configuration is missing";
                _logger.LogInformation(message: log);
            }

            if(issuer == null)
            {
                log = "Issuer configuration is missing";
                _logger.LogInformation(message: log);
            }

            if (signingKey != null && audience != null && issuer != null)
            {
                SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(signingKey));
                SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha512Signature);

                JwtSecurityToken jwtSecurityToken = new(
                    issuer: issuer,
                    claims: claims,
                    audience: audience,
                    expires: DateTime.UtcNow.AddMinutes(expireTimeInMinutesJWT),
                    signingCredentials: credentials
                );

                jwtHandler = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                return jwtHandler;
            }

            jwtHandler = new NotAutorizedResponse();
            log = "Token couldn't be crated";
            _logger.LogInformation(message: log);

            return jwtHandler;
        }
    }
}
