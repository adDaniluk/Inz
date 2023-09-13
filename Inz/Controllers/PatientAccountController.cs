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

            var callback = await _patientService.InsertPatientAsync(patientDTO);

            IActionResult actionResult = callback.Match(
                okResponse => Ok(okResponse.ResponseMessage),
                conflict => Conflict(conflict.ResponseMessage),
                databaseException => Problem($"{databaseException}"));

            return actionResult;
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
                databaseException => Problem($"{databaseException}"));

            return actionResult;
        }
    }
}