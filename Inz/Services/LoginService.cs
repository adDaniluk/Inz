using Inz.DTOModel;
using Inz.Model;
using Inz.OneOfHelper;
using Microsoft.IdentityModel.Tokens;
using OneOf;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Inz.Services
{
    public class LoginService : ILoginService
    {
        private readonly IPasswordHashService _passwordHashService;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public LoginService(IPasswordHashService passwordHashService,
            ILogger<PasswordHashService> logger,
            IConfiguration configuration)
        {
            _passwordHashService = passwordHashService;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<OneOf<OkResponse, NotAutorizedResponse, DatabaseExceptionResponse>> SignIn(LoginDTO loginDTO)
        {
            string log;

            var callbackValidate = await _passwordHashService.ValidatePassword(loginDTO);

            if(!callbackValidate.TryPickT0(out bool isValidated, out var dbError))
            {
                log = $"Error on a database, see inner exception: {dbError.Exception.Message}";
                _logger.LogError(message: log);
                return dbError;
            }

            if(!isValidated)
            {
                log = "Username password does not match!";
                _logger.LogInformation(message: log);
                return new NotAutorizedResponse(log);
            }

            var jwtToken = CreateToken(loginDTO);

            if(jwtToken.TryPickT0(out string token, out var notAutorized))
            {
                log = $"User: {loginDTO.Login} has logged in";
                _logger.LogInformation(message: log);
                return new OkResponse(token);
            }

            log = "JWT could not be created";
            _logger.LogInformation(message: log);
            return new NotAutorizedResponse(log);


        }

        private OneOf<string, NotAutorizedResponse> CreateToken(LoginDTO loginDTO)
        {
            List<Claim> claims = new();
            OneOf<string, NotAutorizedResponse> jwtHandler;
            string log;

            string signingKey = _configuration.GetSection("Token")["ServiceApiKey"]!;
            string audience = _configuration.GetSection("Token")["Audience"]!;
            string issuer = _configuration.GetSection("Token")["Issuer"]!;
            int expireTimeInMinutesJWT = Int16.Parse(_configuration.GetSection("Token")["ExpireTimeInMinutesJWT"]!);

            if (loginDTO.PersonType == PersonType.Doctor)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Doctor"));
            }
            else if (loginDTO.PersonType == PersonType.Patient)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Patient"));
            }

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

            return jwtHandler;
        }
    }
}
