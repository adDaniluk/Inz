using Inz.DTOModel;
using Inz.Helpers;
using Inz.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Patient")]
    public class PatientController : ControllerBase, IPatientController
    {
        private readonly IPatientService _patientService;
        private readonly ILogger _logger;

        public PatientController(IPatientService patientService, ILogger<PatientController> logger)
        {
            _patientService = patientService;
            _logger = logger;
        }

        [Route("profile")]
        [HttpPut]
        public async Task<IActionResult> UpdatePatientAsync(UpdatePatientDTO updatePatientDTO)
        {
            _logger.LogInformation(message: $"Calling {nameof(UpdatePatientAsync)}");

            updatePatientDTO.Id = ClaimsHelper.GetUserIdFromClaims(HttpContext);

            var callback = await _patientService.UpdatePatientAsyc(updatePatientDTO);

            IActionResult actionResult = callback.Match(
                patient => Ok(patient.ResponseMessage),
                notFound => NotFound(notFound.ResponseMessage),
                databaseException => Problem($"{LogHelper.DatabaseErrorController}{databaseException.Exception.Message}"));

            return actionResult;
        }

        [Route("profile")]
        [HttpGet]
        public async Task<IActionResult> GetPatientProfileAsync()
        {
            _logger.LogInformation(message: $"Calling {nameof(GetPatientProfileAsync)}");

            int id = ClaimsHelper.GetUserIdFromClaims(HttpContext);

            var callback = await _patientService.GetPatientProfileAsync(id);

            IActionResult actionResult = callback.Match(
                patient => Ok(patient),
                notFound => NotFound(notFound.ResponseMessage),
                databaseException => Problem($"{LogHelper.DatabaseErrorController}{databaseException.Exception.Message}"));

            return actionResult;
        }
    }
}