using Inz.Context;
using Inz.DTOModel;
using Inz.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAccountController : ControllerBase
    {
        private readonly IUserServices _userServices;

        [HttpPost]
        public async Task<IActionResult> InsertNewUserAsync(PatientDTO patientDTO, AddressDTO addressDTO)
        {
            await _userServices.RegisterUser(patientDTO, addressDTO);
            return Ok();
        }

    }
}
