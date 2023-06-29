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

            IActionResult actionResult = returnValue.Match(
                patient => Ok("New user has been created"),
                databaseException => Problem("Cannot connect to the database, please contact Admin@admin.admin | " +
                    $"See inner exception: {databaseException.exception.Message}"));

            return actionResult;
        }

        [Route("api/[controller]/{id}")]
        [HttpPut]
        public async Task<IActionResult> UpdatePatientAsync(UpdatePatientDTO updatePatientDTO, int id)
        {
            var returnValue = await _patientService.UpdatePatientAsync(updatePatientDTO, id);

            IActionResult actionResult = returnValue.Match(
                patient => Ok("User has been updated"),
                notFound => NotFound($"User with {id} does not exist."),
                databaseException => Problem("Cannot connect to the database, please contact Admin@admin.admin | " +
                    $"See inner exception: {databaseException.exception.Message}"));

            return actionResult;
        }
    }
}
