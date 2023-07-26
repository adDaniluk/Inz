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
            _logger.LogInformation($"Calling {nameof(InsertDoctorAsync)}");

            var callback = await _doctorService.InsertDoctorAsync(doctorDTO);

            IActionResult actionResult = callback.Match(
                doctor => Ok(doctor.ResponseMessage),
                notValidate => BadRequest(notValidate.ValidationResult.Errors.ToList().Select(x => new { Error = $"{x.ErrorCode}: {x.ErrorMessage}" })),
                databaseException => Problem($"{databaseException.Exception.Message}"));

            return actionResult;
        }

        [Route("Update")]
        [HttpPut]
        public async Task<IActionResult> UpdateDoctorAsync(UpdateDoctorDTO updateDoctorDTO)
        {
            _logger.LogInformation($"Calling {nameof(UpdateDoctorAsync)}");

            var callback = await _doctorService.UpdateDoctorAsync(updateDoctorDTO);

            IActionResult actionResult = callback.Match(
                okResult => Ok(okResult.ResponseMessage),
                notFound => NotFound(notFound.ResponseMessage),
                notValidate => BadRequest(notValidate.ValidationResult.Errors.ToList().Select(x => new { Error = $"{x.ErrorCode}: {x.ErrorMessage}" })),
                databaseException => Problem($"{databaseException.Exception.Message}"));

            return actionResult;
        }

        [Route("AddService")]
        [HttpPost]
        public async Task<IActionResult> AddDoctorServiceAsync(ServiceDoctorDTO serviceDoctorDTO)
        {
            _logger.LogInformation($"Calling {nameof(AddDoctorServiceAsync)}");

            //ServiceDoctorDTOValidator serviceDTOValidator = new ServiceDoctorDTOValidator();
            //var validatorResult = serviceDTOValidator.Validate(serviceDoctorDTO);

            //if (!validatorResult.IsValid)
            //{
            //    return BadRequest(validatorResult.Errors.ToList().Select(x => new { Error = $"{x.ErrorCode}: {x.ErrorMessage}" }));
            //}

            var callback = await _doctorService.AddDoctorServiceAsync(serviceDoctorDTO);

            IActionResult actionResult = callback.Match(
                doctorServices => Ok(doctorServices.ResponseMessage),
                notFound => NotFound(notFound.ResponseMessage),
                notValidate => BadRequest(notValidate.ValidationResult.Errors.ToList().Select(x => new { Error = $"{x.ErrorCode}: {x.ErrorMessage}" })),
                databaseException => Problem("Cannot connect to the database, please contact Admin@admin.admin | " +
                    $"See inner exception: {databaseException.Exception.Message}"));

            return actionResult;
        }

        //[Route("Service")]
        //[HttpDelete]
        //public async Task<IActionResult> RemoveDoctorServiceAsync(ServiceDoctorDTO serviceDoctorDTO)
        //{ 
        //    //TODO what about DTO?
        //    RemoveServiceDoctorDTOValidator serviceDTOValidator = new RemoveServiceDoctorDTOValidator();
        //    var validatorResult = serviceDTOValidator.Validate(serviceDoctorDTO);

        //    if (!validatorResult.IsValid)
        //    {
        //        return BadRequest(validatorResult.Errors.ToList().Select(x => new { Error = $"{x.ErrorCode}: {x.ErrorMessage}" }));
        //    }

        //    var returnValue = await _doctorService.RemoveDoctorServiceAsync(serviceDoctorDTO);

        //    IActionResult actionResult = returnValue.Match(
        //        doctorServices => Ok(doctorServices.ResponseMessage),
        //        notFound => NotFound(notFound.ResponseMessage),
        //        notValidate => BadRequest(validatorResult.Errors.ToList().Select(x => new { Error = $"{x.ErrorCode}: {x.ErrorMessage}" })),
        //        databaseException => Problem("Cannot connect to the database, please contact Admin@admin.admin | " +
        //            $"See inner exception: {databaseException.Exception.Message}"));

        //    return actionResult;
        //}
    }
}
