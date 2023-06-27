using Inz.DTOModel;
using Inz.Model;
using Inz.OneOfHelper;
using Inz.Services;
using Microsoft.AspNetCore.Mvc;
using OneOf;

namespace Inz.Controllers
{
    public class DoctorAccountController : ControllerBase, IDoctorAccountController
    {
        private readonly IDoctorService _doctorService;

        public DoctorAccountController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateDoctorAsync(DoctorDTO doctorDTO)
        {
            OneOf<Doctor, DatabaseException> returnValue = await _doctorService.InsertNewDoctorAsync(doctorDTO);

            IActionResult actionResult = returnValue.Match(
                doctor => Ok("New Doctor has been created."),
                DatabaseException => Problem("Cannot connect to the database, please contact Admin@admin.admin | " +
                    $"See inner exception: {DatabaseException.exception.Message}"));

            return actionResult;
        }
    }
}
