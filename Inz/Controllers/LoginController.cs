using Inz.DTOModel;
using Inz.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Inz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase, ILoginController
    {
        private readonly ILoginService _loginService;
        private readonly ILogger _logger;
        
        public LoginController(ILoginService loginService, ILogger<LoginController> logger)
        {
            _loginService = loginService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginDTO loginDTO)
        {
            _logger.LogInformation(message: $"Calling {nameof(LoginAsync)}");

            var callback = await _loginService.SignIn(loginDTO);

            IActionResult response =  callback.Match(
                okResponse => Ok(okResponse.ResponseMessage),
                notAutorizedResponse => Ok(notAutorizedResponse.ReponseMessage),
                databaseException => Problem(databaseException.Exception.Message)
                );

            return response;
        }
    }
}
