using Inz.DTOModel;
using Microsoft.AspNetCore.Mvc;

namespace Inz.Controllers
{
    public interface IPatientAccountController
    {
        public Task<IActionResult> InsertPatientAsync(PatientDTO patientDTO);

        public Task<IActionResult> UpdatePatientAsync(UpdatePatientDTO updatePatientDTO);
    }
}
