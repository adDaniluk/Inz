using Inz.DTOModel;
using Inz.DTOModel.Validators;
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
            PatientDTOValidator patientDTOvalidator = new PatientDTOValidator();
            var validatorResult = patientDTOvalidator.Validate(patientDTO);

            if(!validatorResult.IsValid)
            {
                return BadRequest(validatorResult.Errors.ToList().Select(x => new { Error = $"{x.ErrorCode}: {x.ErrorMessage}" }));
            }

            var returnValue = await _patientService.InsertPatientAsync(patientDTO);

            IActionResult actionResult = returnValue.Match(
                patient => Ok(patient.Response),
                databaseException => Problem("Cannot connect to the database, please contact Admin@admin.admin | " +
                    $"See inner exception: {databaseException.Exception.Message}"));

            return actionResult;
        }

        [Route("Update")]
        [HttpPut]
        public async Task<IActionResult> UpdatePatientAsync(UpdatePatientDTO updatePatientDTO)
        {

            UpdatePatientDTOValidator updatePatientDTOValidator = new UpdatePatientDTOValidator();
            var validatorResult = updatePatientDTOValidator.Validate(updatePatientDTO);

            if (!validatorResult.IsValid)
            {
                return BadRequest(validatorResult.Errors.ToList().Select(x => new { Error = $"{x.ErrorCode}: {x.ErrorMessage}" }));
            }

            var returnValue = await _patientService.UpdatePatientAsyc(updatePatientDTO);

            IActionResult actionResult = returnValue.Match(
                patient => Ok(patient.Response),
                notFound => NotFound(notFound.Response),
                databaseException => Problem("Cannot connect to the database, please contact Admin@admin.admin | " +
                    $"See inner exception: {databaseException.Exception.Message}"));

            return actionResult;
        }
    }
}