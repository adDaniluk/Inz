using Inz.DTOModel;
using Inz.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Doctor")]
    public class DoctorAccountController : ControllerBase, IDoctorAccountController
    {
        private readonly IDoctorService _doctorService;
        private readonly ILogger _logger;

        public const string dbErrorInformation = "Cannot connect to the database, please contact Admin@admin.admin. See inner exception:";

        public DoctorAccountController(IDoctorService doctorService, ILogger<IDoctorAccountController> logger)
        {
            _doctorService = doctorService;
            _logger = logger;
        }

        [Route("Update")]
        [HttpPut]
        public async Task<IActionResult> UpdateDoctorAsync(UpdateDoctorDTO updateDoctorDTO)
        {
            _logger.LogInformation(message: $"Calling {nameof(UpdateDoctorAsync)}");

            var callback = await _doctorService.UpdateDoctorAsync(updateDoctorDTO);

            IActionResult actionResult = callback.Match(
                okResult => Ok(okResult.ResponseMessage),
                notFound => NotFound(notFound.ResponseMessage),
                databaseException => Problem($"{dbErrorInformation}: {databaseException.Exception.Message}"));

            return actionResult;
        }

        [Route("AddService")]
        [HttpPost]
        public async Task<IActionResult> AddDoctorServiceAsync(DoctorServiceDTO serviceDoctorDTO)
        {
            _logger.LogInformation($"Calling {nameof(AddDoctorServiceAsync)}");

            var callback = await _doctorService.AddDoctorServiceAsync(serviceDoctorDTO);

            IActionResult actionResult = callback.Match(
                doctorServices => Ok(doctorServices.ResponseMessage),
                notFound => NotFound(notFound.ResponseMessage),
                databaseException => Problem($"{dbErrorInformation}: {databaseException.Exception.Message}"));

            return actionResult;
        }

        [Route("RemoveService")]
        [HttpDelete]
        public async Task<IActionResult> RemoveDoctorServiceAsync(RemoveDoctorServiceDTO removeDoctorServiceDTO)
        {
            _logger.LogInformation($"Calling {nameof(RemoveDoctorServiceAsync)}");

            var callback = await _doctorService.RemoveDoctorServiceAsync(removeDoctorServiceDTO);

            IActionResult actionResult = callback.Match(
                doctorServices => Ok(doctorServices.ResponseMessage),
                notFound => NotFound(notFound.ResponseMessage),
                databaseException => Problem($"{dbErrorInformation}: {databaseException.Exception.Message}"));

            return actionResult;
        }
    }
}
