using Inz.DTOModel;
using Inz.Model;
using Inz.OneOfHelper;
using Microsoft.IdentityModel.Tokens;
using OneOf;
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

            string? jwtToken = CreateToken(loginDTO);

            if (jwtToken == null)
            {
                log = "Secret api key is missing to create JWT.";
                _logger.LogInformation(message: log);
                return new NotAutorizedResponse(log);
            }

            log = $"User: {loginDTO.Login} has logged in";
            _logger.LogInformation(message: log);
            return new OkResponse(jwtToken);
        }

        private string? CreateToken(LoginDTO loginDTO)
        {
            List<Claim> claims = new();

            string? apiKey = _configuration["Inz:ServiceApiKey"];

            SymmetricSecurityKey key;
            SigningCredentials credentials;
            JwtSecurityToken jwtSecurityToken;
            string jwtHandler;
            
            if (loginDTO.PersonType == PersonType.Doctor)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Doctor"));
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, "Patient"));
            }

            if (apiKey == null)
            {
                return null;
            }

            key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(apiKey));
            credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            jwtSecurityToken = new JwtSecurityToken(
                issuer: loginDTO.Login,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: credentials
            );

            jwtHandler = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return jwtHandler;
        }
    }
}
