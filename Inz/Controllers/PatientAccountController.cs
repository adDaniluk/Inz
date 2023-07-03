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

        public PatientAccountController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpPost]
        public async Task<IActionResult> InsertPatientAsync(PatientDTO patientDTO)
        {
            PatientDTOValidator patientDTOvalidator = new PatientDTOValidator();
            var validatorResult = patientDTOvalidator.Validate(patientDTO);

            if(!validatorResult.IsValid)
            {
                return BadRequest(validatorResult.Errors.ToList().Select(x => x.ErrorMessage));
            }

            var returnValue = await _patientService.InsertPatientAsync(patientDTO);

            IActionResult actionResult = returnValue.Match(
                patient => Ok("New user has been created."),
                databaseException => Problem("Cannot connect to the database, please contact Admin@admin.admin | " +
                    $"See inner exception: {databaseException.exception.Message}"));

            return actionResult;
        }

        [Route("api/[controller]/update")]
        [HttpPut]
        public async Task<IActionResult> UpdatePatientAsync(UpdatePatientDTO updatePatientDTO)
        {

            UpdatePatientDTOValidator updatePatientDTOValidator = new UpdatePatientDTOValidator();
            var validatorResult = updatePatientDTOValidator.Validate(updatePatientDTO);

            if (!validatorResult.IsValid)
            {
                return BadRequest(validatorResult.Errors.ToList().Select(x => x.ErrorMessage));
            }

            var returnValue = await _patientService.UpdatePatientAsyc(updatePatientDTO);

            IActionResult actionResult = returnValue.Match(
                patient => Ok("User has been updated."),
                notFound => NotFound($"User with {updatePatientDTO.Id} does not exist."),
                databaseException => Problem("Cannot connect to the database, please contact Admin@admin.admin | " +
                    $"See inner exception: {databaseException.exception.Message}"));

            return actionResult;
        }
    }
}
