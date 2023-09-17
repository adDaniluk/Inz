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

            string signingKey = _configuration.GetSection("Token")["ServiceApiKey"]!;
            string audience = _configuration.GetSection("Token")["Audience"]!;
            string issuer = _configuration.GetSection("Token")["Issuer"]!;
            
            if (loginDTO.PersonType == PersonType.Doctor)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Doctor"));
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, "Patient"));
            }

            if (signingKey == null || audience == null || issuer == null)
            {
                return null;
            }

            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(signingKey));
            SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha512Signature);

            JwtSecurityToken jwtSecurityToken = new(
                issuer: issuer,
                claims: claims,
                audience: audience,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: credentials
            );

            string jwtHandler = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return jwtHandler;
        }
    }
}
