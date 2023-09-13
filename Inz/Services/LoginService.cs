namespace Inz.Services
{
    public class LoginService : ILoginService
    {
        private readonly IPasswordHashService _passwordHashService;

        public LoginService(IPasswordHashService passwordHashService)
        {
            _passwordHashService = passwordHashService;
        }


    }
}
