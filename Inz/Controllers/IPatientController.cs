using Inz.DTOModel;
using Microsoft.AspNetCore.Mvc;

namespace Inz.Controllers
{
    public interface IPatientController
    {
        public Task<IActionResult> UpdatePatientAsync(UpdatePatientDTO updatePatientDTO);
        public Task<IActionResult> GetPatientProfileAsync();
    }
}
