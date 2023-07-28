using Inz.DTOModel;
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
                databaseException => Problem($"{databaseException.Exception.Message}"));

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
                databaseException => Problem("Cannot connect to the database, please contact Admin@admin.admin | " +
                    $"See inner exception: {databaseException.Exception.Message}"));

            return actionResult;
        }

        [Route("RemoveService")]
        [HttpDelete]
        public async Task<IActionResult> RemoveDoctorServiceAsync(RemoveDoctorServiceDTO removeDoctorServiceDTO)
        {
            _logger.LogInformation($"Calling {nameof(RemoveDoctorServiceAsync)}");

            var returnValue = await _doctorService.RemoveDoctorServiceAsync(removeDoctorServiceDTO);

            IActionResult actionResult = returnValue.Match(
                doctorServices => Ok(doctorServices.ResponseMessage),
                notFound => NotFound(notFound.ResponseMessage),
                databaseException => Problem("Cannot connect to the database, please contact Admin@admin.admin | " +
                    $"See inner exception: {databaseException.Exception.Message}"));

            return actionResult;
        }
    }
}
