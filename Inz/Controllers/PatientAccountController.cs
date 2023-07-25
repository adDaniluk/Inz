using Inz.DTOModel;
using Inz.Services;
using Microsoft.AspNetCore.Mvc;

namespace Inz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientAccountController : ControllerBase, IPatientAccountController
    {
        private readonly IPatientService _patientService;
        private readonly ILogger _logger;

        public PatientAccountController(IPatientService patientService, ILogger<PatientAccountController> logger)
        {
            _patientService = patientService;
            _logger = logger;
        }

        [Route("AddPatient")]
        [HttpPost]
        public async Task<IActionResult> InsertPatientAsync(PatientDTO patientDTO)
        {
            _logger.LogInformation(message: $"Calling: {nameof(InsertPatientAsync)}");
            
            var returnValue = await _patientService.InsertPatientAsync(patientDTO);

            IActionResult actionResult = returnValue.Match(
                okResponse => Ok(okResponse.ResponseMessage),
                notValidate => BadRequest(notValidate.ValidationResult.Errors.ToList().Select(x => new { Error = $"{x.ErrorCode}: {x.ErrorMessage}" })),
                databaseException => Problem($"{databaseException.Exception}"));

            return actionResult;
        }

        [Route("Update")]
        [HttpPut]
        public async Task<IActionResult> UpdatePatientAsync(UpdatePatientDTO updatePatientDTO)
        {
            _logger.LogInformation(message: $"Calling {nameof(UpdatePatientAsync)}");
            
            var returnValue = await _patientService.UpdatePatientAsyc(updatePatientDTO);

            IActionResult actionResult = returnValue.Match(
                patient => Ok(patient.ResponseMessage),
                notFound => NotFound(notFound.ResponseMessage),
                notValidate => BadRequest(notValidate.ValidationResult.Errors.ToList().Select(x => new { Error = $"{x.ErrorCode}: {x.ErrorMessage}" })),
                databaseException => Problem("Cannot connect to the database, please contact Admin@admin.admin | " +
                    $"See inner exception: {databaseException}"));

            return actionResult;
        }
    }
}