using Inz.Context;
using Inz.DTOModel;
using Inz.Services;
using Microsoft.AspNetCore.Mvc;

namespace Inz.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegisterController : ControllerBase, IRegisterControler 
    {
        private readonly DbContextApi _dbContextApi;
        private readonly ILogger _logger;
        private readonly PatientService _patientService;
        private readonly DoctorService _doctorService;

        public RegisterController(DbContextApi dbContextApi,
            DoctorService doctorService,
            PatientService patientService,
            ILogger logger)
        {
            _dbContextApi = dbContextApi;
            _doctorService = doctorService;
            _patientService = patientService;
            _logger = logger;
        }

        [Route("DoctorRegister")]
        [HttpPost]
        public async Task<IActionResult> DoctorRegisterAsync(DoctorDTO doctorDTO)
        {
            _logger.LogInformation($"Calling {nameof(DoctorRegisterAsync)}");

            var callback = await _doctorService.InsertDoctorAsync(doctorDTO);

            IActionResult actionResult = callback.Match(
                doctor => Ok(doctor.ResponseMessage),
                conflict => Conflict(conflict.ResponseMessage),
                databaseException => Problem($"{databaseException.Exception.Message}"));

            return actionResult;
        }

        [Route("PatientRegister")]
        [HttpPost]
        public async Task<IActionResult> PatientRegisterAsync(PatientDTO patientDTO)
        {
            _logger.LogInformation(message: $"Calling: {nameof(PatientRegisterAsync)}");

            var callback = await _patientService.InsertPatientAsync(patientDTO);

            IActionResult actionResult = callback.Match(
                okResponse => Ok(okResponse.ResponseMessage),
                conflict => Conflict(conflict.ResponseMessage),
                databaseException => Problem($"{databaseException}"));

            return actionResult;
        }
    }
}
