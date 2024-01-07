using Inz.Context;
using Inz.DTOModel;
using Inz.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inz.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class RegisterController : ControllerBase, IRegisterControler 
    {
        private readonly DbContextApi _dbContextApi;
        private readonly ILogger<RegisterController> _logger;
        private readonly IPatientService _patientService;
        private readonly IDoctorService _doctorService;

        public const string dbErrorInformation = "Cannot connect to the database, please contact Admin@admin.admin. See inner exception:";

        public RegisterController(DbContextApi dbContextApi,
            IDoctorService doctorService,
            IPatientService patientService,
            ILogger<RegisterController> logger)
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
            _logger.LogInformation(message: $"Calling {nameof(DoctorRegisterAsync)}");

            var callback = await _doctorService.InsertDoctorAsync(doctorDTO);

            IActionResult actionResult = callback.Match(
                doctor => Ok(doctor.ResponseMessage),
                conflict => Conflict(conflict.ResponseMessage),
                databaseException => Problem($"{dbErrorInformation}: {databaseException.Exception.Message}"));

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
                databaseException => Problem($"{dbErrorInformation}: {databaseException.Exception.Message}"));

            return actionResult;
        }
    }
}
