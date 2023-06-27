using Inz.DTOModel;
using Microsoft.AspNetCore.Mvc;

namespace Inz.Controllers
{
    public interface IDoctorAccountController
    {
        public Task<IActionResult> CreateDoctorAsync(DoctorDTO doctorDTO);
    }
}
