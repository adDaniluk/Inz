using Inz.DTOModel;
using Inz.DTOModel.Validators;
using Inz.Services;
using Microsoft.AspNetCore.Mvc;

namespace Inz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorAccountController : ControllerBase, IDoctorAccountController
    {
        private readonly IDoctorService _doctorService;
        private readonly ILogger _logger;

        public DoctorAccountController(IDoctorService doctorService, ILogger<IDoctorAccountController> logger)
        {
            _doctorService = doctorService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> InsertDoctorAsync(DoctorDTO doctorDTO)
        {
            DotorDTOValidator doctorDTOValidator = new DotorDTOValidator();
            var validatorResult = doctorDTOValidator.Validate(doctorDTO);

            if (!validatorResult.IsValid)
            {
                _logger.LogError($"DTO object is not valid: {validatorResult.Errors.First()}");
                return BadRequest(validatorResult.Errors.ToList().Select(x => new { Error = $"{x.ErrorCode}: {x.ErrorMessage}"}));
            }

            var returnValue = await _doctorService.InsertDoctorAsync(doctorDTO);

            IActionResult actionResult = returnValue.Match(
                doctor => Ok(doctor.Response),
                DatabaseException => Problem("Cannot connect to the database, please contact Admin@admin.admin | " +
                    $"See inner exception: {DatabaseException.Exception.Message}"));

            return actionResult;
        }

        [Route("Update")]
        [HttpPut]
        public async Task<IActionResult> UpdateDoctorAsync(UpdateDoctorDTO updateDoctorDTO)
        {
            UpdateDoctorDTOValidator updateDoctorDTOvalidator = new UpdateDoctorDTOValidator();
            var validatorResult = updateDoctorDTOvalidator.Validate(updateDoctorDTO);

            if(!validatorResult.IsValid)
            {
                return BadRequest(validatorResult.Errors.ToList().Select(x => new { Error = $"{x.ErrorCode}: {x.ErrorMessage}" }));
            }

            var returnValue = await _doctorService.UpdateDoctorAsync(updateDoctorDTO);

            IActionResult actionResult = returnValue.Match(
                patient => Ok(patient.Response),
                notFound => NotFound(notFound.Response),
                databaseException => Problem("Cannot connect to the database, please contact Admin@admin.admin | " +
                    $"See inner exception: {databaseException.Exception.Message}"));

            return actionResult;
        }

        [Route("Service")]
        [HttpPost]
        public async Task<IActionResult> AddDoctorServiceAsync(ServiceDoctorDTO serviceDoctorDTO)
        {
            ServiceDoctorDTOValidator serviceDTOValidator = new ServiceDoctorDTOValidator();
            var validatorResult = serviceDTOValidator.Validate(serviceDoctorDTO);

            if(!validatorResult.IsValid)
            {
                return BadRequest(validatorResult.Errors.ToList().Select(x => new { Error = $"{x.ErrorCode}: {x.ErrorMessage}" }));
            }

            var returnValue = await _doctorService.AddDoctorServiceAsync(serviceDoctorDTO);

            IActionResult actionResult = returnValue.Match(
                doctorServices => Ok(doctorServices.Response),
                notFound => NotFound(notFound.Response),
                databaseException => Problem("Cannot connect to the database, please contact Admin@admin.admin | " +
                    $"See inner exception: {databaseException.Exception.Message}"));

            return actionResult;
        }

        [Route("Service")]
        [HttpDelete]
        public async Task<IActionResult> RemoveDoctorServiceAsync(ServiceDoctorDTO serviceDoctorDTO)
        { 
            //TODO what about DTO?
            RemoveServiceDoctorDTOValidator serviceDTOValidator = new RemoveServiceDoctorDTOValidator();
            var validatorResult = serviceDTOValidator.Validate(serviceDoctorDTO);

            if (!validatorResult.IsValid)
            {
                return BadRequest(validatorResult.Errors.ToList().Select(x => new { Error = $"{x.ErrorCode}: {x.ErrorMessage}" }));
            }

            var returnValue = await _doctorService.RemoveDoctorServiceAsync(serviceDoctorDTO);

            IActionResult actionResult = returnValue.Match(
                doctorServices => Ok(doctorServices.Response),
                notFound => NotFound(notFound.Response),
                databaseException => Problem("Cannot connect to the database, please contact Admin@admin.admin | " +
                    $"See inner exception: {databaseException.Exception.Message}"));

            return actionResult;
        }
    }
}
