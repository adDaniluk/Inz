using Inz.DTOModel;
using Inz.DTOModel.Validators;
using Inz.Model;
using Inz.OneOfHelper;
using Inz.Services;
using Microsoft.AspNetCore.Mvc;
using OneOf;

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

            OneOf<Doctor, DatabaseException> returnValue = await _doctorService.InsertDoctorAsync(doctorDTO);

            IActionResult actionResult = returnValue.Match(
                doctor => Ok("New Doctor has been created."),
                DatabaseException => Problem("Cannot connect to the database, please contact Admin@admin.admin | " +
                    $"See inner exception: {DatabaseException.exception.Message}"));

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
                patient => Ok("Doctor has been updated."),
                notFound => NotFound($"Doctor with {updateDoctorDTO.Id} does not exist."),
                databaseException => Problem("Cannot connect to the database, please contact Admin@admin.admin | " +
                    $"See inner exception: {databaseException.exception.Message}"));

            return actionResult;
        }

        [Route("AddService")]
        [HttpPost]
        public async Task<IActionResult> AddDoctorServiceAsync(ServiceDoctorDTO serviceDTO)
        {
            ServiceDoctorDTOValidator serviceDTOValidator = new ServiceDoctorDTOValidator();
            var validatorResult = serviceDTOValidator.Validate(serviceDTO);

            if(!validatorResult.IsValid)
            {
                return BadRequest(validatorResult.Errors.ToList().Select(x => new { Error = $"{x.ErrorCode}: {x.ErrorMessage}" }));
            }

            var returnValue = await _doctorService.AddDoctorServiceAsync(serviceDTO);

            IActionResult actionResult = returnValue.Match(
                doctorServices => Ok("Service has been added."),
                notFound => NotFound("A service or doctor does not exist."),
                databaseException => Problem("Cannot connect to the database, please contact Admin@admin.admin | " +
                    $"See inner exception: {databaseException.exception.Message}"));

            return actionResult;
        }
    }
}
