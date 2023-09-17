using Inz.DTOModel;
using Microsoft.AspNetCore.Mvc;

namespace Inz.Controllers
{
    public interface IRegisterControler
    {
        public Task<IActionResult> DoctorRegisterAsync(DoctorDTO doctorDTO);
        public Task<IActionResult> PatientRegisterAsync(PatientDTO patientDTO);
    }
}
