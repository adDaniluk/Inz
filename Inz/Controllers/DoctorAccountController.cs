using Inz.DTOModel;
using Inz.DTOModel.Validators;
using Inz.Model;
using Inz.OneOfHelper;
using Inz.Services;
using Microsoft.AspNetCore.Mvc;
using OneOf;

namespace Inz.Controllers
{
    [Route("api/[controller]/Doctor")]
    [ApiController]
    public class DoctorAccountController : ControllerBase, IDoctorAccountController
    {
        private readonly IDoctorService _doctorService;

        public DoctorAccountController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpPost]
        public async Task<IActionResult> InsertDoctorAsync(DoctorDTO doctorDTO)
        {
            DotorDTOValidator doctorDTOValidator = new DotorDTOValidator();
            var validatorResult = doctorDTOValidator.Validate(doctorDTO);

            if (!validatorResult.IsValid)
            {
                return BadRequest(validatorResult.Errors.ToList().Select(x => new { x.ErrorMessage, x.ErrorCode }));;
            }

            OneOf<Doctor, DatabaseException> returnValue = await _doctorService.InsertDoctorAsync(doctorDTO);

            IActionResult actionResult = returnValue.Match(
                doctor => Ok("New Doctor has been created."),
                DatabaseException => Problem("Cannot connect to the database, please contact Admin@admin.admin | " +
                    $"See inner exception: {DatabaseException.exception.Message}"));

            return actionResult;
        }

        [Route("api/[controller]/DoctorUpdate")]
        [HttpPut]
        public async Task<IActionResult> UpdateDoctorAsync(UpdateDoctorDTO updateDoctorDTO)
        {
            UpdateDoctorDTOValidator updateDoctorDTOvalidator = new UpdateDoctorDTOValidator();
            var validatorResult = updateDoctorDTOvalidator.Validate(updateDoctorDTO);

            if(!validatorResult.IsValid)
            {
                return BadRequest(validatorResult.Errors.ToList().Select(x => x.ErrorMessage));
            }

            var returnValue = await _doctorService.UpdateDoctorAsync(updateDoctorDTO);

            IActionResult actionResult = returnValue.Match(
                patient => Ok("Doctor has been updated."),
                notFound => NotFound($"Doctor with {updateDoctorDTO.Id} does not exist."),
                databaseException => Problem("Cannot connect to the database, please contact Admin@admin.admin | " +
                    $"See inner exception: {databaseException.exception.Message}"));

            return actionResult;
        }
    }
}
