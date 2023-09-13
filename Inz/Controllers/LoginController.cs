using Inz.DTOModel;
using Inz.Services;
using Microsoft.AspNetCore.Mvc;

namespace Inz.Controllers
{
    public class LoginController : ControllerBase, ILoginController
    {
        private readonly ILoginService _loginService;
        private readonly ILogger _logger;
        
        public LoginController(ILoginService loginService, ILogger<LoginController> logger)
        {
            _loginService = loginService;
            _logger = logger;
        }

        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginDTO loginDTO)
        {
            _logger.LogInformation(message: $"Calling {nameof(LoginAsync)}");

            //var callback = await _loginService

            return Ok();
        }
    }
}
