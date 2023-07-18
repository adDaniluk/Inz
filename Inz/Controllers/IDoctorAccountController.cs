using Inz.DTOModel;
using Microsoft.AspNetCore.Mvc;

namespace Inz.Controllers
{
    public interface IDoctorAccountController
    {
        public Task<IActionResult> InsertDoctorAsync(DoctorDTO doctorDTO);
        public Task<IActionResult> UpdateDoctorAsync(UpdateDoctorDTO updateDoctorDTO);
        public Task<IActionResult> AddDoctorServiceAsync(ServiceDoctorDTO serviceDTO);
        public Task<IActionResult> RemoveDoctorServiceAsync(ServiceDoctorDTO serviceDoctorDTO);
    }
}
