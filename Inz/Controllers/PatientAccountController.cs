using Inz.DTOModel;
using Inz.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Patient")]
    public class PatientAccountController : ControllerBase, IPatientAccountController
    {
        private readonly IPatientService _patientService;
        private readonly ILogger _logger;

        public const string dbErrorInformation = "Cannot connect to the database, please contact Admin@admin.admin. See inner exception:";

        public PatientAccountController(IPatientService patientService, ILogger<PatientAccountController> logger)
        {
            _patientService = patientService;
            _logger = logger;
        }

        [Route("Update")]
        [HttpPut]
        public async Task<IActionResult> UpdatePatientAsync(UpdatePatientDTO updatePatientDTO)
        {
            _logger.LogInformation(message: $"Calling {nameof(UpdatePatientAsync)}");
            
            var callback = await _patientService.UpdatePatientAsyc(updatePatientDTO);

            IActionResult actionResult = callback.Match(
                patient => Ok(patient.ResponseMessage),
                notFound => NotFound(notFound.ResponseMessage),
                databaseException => Problem($"{dbErrorInformation}: {databaseException.Exception.Message}"));

            return actionResult;
        }
    }
}