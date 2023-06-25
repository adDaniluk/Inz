using Inz.DTOModel;
using Inz.OneOfHelper;
using Inz.Services;
using Microsoft.AspNetCore.Mvc;

namespace Inz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientAccountController : ControllerBase, IPatientAccountController
    {
        private readonly IPatientService _patientService;

        public PatientAccountController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePatientAsync(PatientDTO patientDTO)
        {
            var returnValue = await _patientService.InsertPatientAsync(patientDTO);
            if(returnValue.Value.GetType() == typeof(DisconnectFromDatabase))
            {
                return Problem("Cannot connect to the database, please contact Admin@test.com");
            }
            return Ok("New user has been created");
        }

    }
}
