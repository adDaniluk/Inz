using Inz.DTOModel;
using Inz.Helpers;
using Inz.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inz.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
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
                databaseException => Problem($"{LogHelper.DatabaseErrorController}{databaseException.Exception.Message}"));

            return response;
        }
    }
}
