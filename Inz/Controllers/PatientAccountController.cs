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

            if(returnValue.Value.GetType() == typeof(DatabaseException))
            {
                return Problem("Cannot connect to the database, please contact Admin@admin.admin | "+
                    $"See inner exception: {returnValue.AsT1._message}");
            }
            return Ok("New user has been created");
        }

    }
}
