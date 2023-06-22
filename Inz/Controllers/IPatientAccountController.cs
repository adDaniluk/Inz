﻿using Inz.DTOModel;
using Microsoft.AspNetCore.Mvc;

namespace Inz.Controllers
{
    public interface IPatientAccountController
    {
        public Task<IActionResult> CreatePatientAsync(PatientDTO patientDTO);
    }
}
