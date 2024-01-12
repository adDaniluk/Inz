using Inz.DTOModel;
using Microsoft.AspNetCore.Mvc;

namespace Inz.Controllers
{
    public interface IDoctorController
    {
        public Task<IActionResult> UpdateDoctorAsync(UpdateDoctorDTO updateDoctorDTO);
        public Task<IActionResult> AddDoctorServiceAsync(DoctorServiceDTO serviceDTO);
        public Task<IActionResult> RemoveDoctorServiceAsync(RemoveDoctorServiceDTO removeDoctorServiceDTO);
        public Task<IActionResult> GetDoctorAsync();
    }
}
