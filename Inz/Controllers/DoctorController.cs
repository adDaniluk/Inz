﻿using Inz.DTOModel;
using Inz.Helpers;
using Inz.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Doctor")]
    public class DoctorController : ControllerBase, IDoctorController
    {
        private readonly IDoctorService _doctorService;
        private readonly ILogger _logger;

        public DoctorController(IDoctorService doctorService,
            ILogger<IDoctorController> logger)
        {
            _doctorService = doctorService;
            _logger = logger;
        }

        [Route("profile")]
        [HttpPut]
        public async Task<IActionResult> UpdateDoctorAsync(UpdateDoctorDTO updateDoctorDTO)
        {
            _logger.LogInformation(message: $"Calling {nameof(UpdateDoctorAsync)}");

            updateDoctorDTO.Id = ClaimsHelper.GetUserIdFromClaims(HttpContext);

            var callback = await _doctorService.UpdateDoctorAsync(updateDoctorDTO);

            IActionResult actionResult = callback.Match(
                okResult => Ok(okResult.ResponseMessage),
                notFound => Conflict(notFound.ResponseMessage),
                databaseException => Problem($"{LogHelper.DatabaseErrorController}{databaseException.Exception.Message}"));

            return actionResult;
        }

        [Route("service")]
        [HttpPost]
        public async Task<IActionResult> AddDoctorServiceAsync(DoctorServiceDTO serviceDoctorDTO)
        {
            _logger.LogInformation($"Calling {nameof(AddDoctorServiceAsync)}");

            serviceDoctorDTO.DoctorId = ClaimsHelper.GetUserIdFromClaims(HttpContext);

            var callback = await _doctorService.AddDoctorServiceAsync(serviceDoctorDTO);

            IActionResult actionResult = callback.Match(
                doctorServices => Ok(doctorServices.ResponseMessage),
                notFound => Conflict(notFound.ResponseMessage),
                databaseException => Problem($"{LogHelper.DatabaseErrorController}{databaseException.Exception.Message}"));

            return actionResult;
        }

        [Route("service")]
        [HttpDelete]
        public async Task<IActionResult> RemoveDoctorServiceAsync(RemoveDoctorServiceDTO removeDoctorServiceDTO)
        {
            _logger.LogInformation($"Calling {nameof(RemoveDoctorServiceAsync)}");

            removeDoctorServiceDTO.DoctorId = ClaimsHelper.GetUserIdFromClaims(HttpContext);

            var callback = await _doctorService.RemoveDoctorServiceAsync(removeDoctorServiceDTO);

            IActionResult actionResult = callback.Match(
                doctorServices => Ok(doctorServices.ResponseMessage),
                notFound => Conflict(notFound.ResponseMessage),
                databaseException => Problem($"{LogHelper.DatabaseErrorController}{databaseException.Exception.Message}"));

            return actionResult;
        }

        [Route("profile")]
        [HttpGet]
        public async Task<IActionResult> GetDoctorAsync()
        {
            _logger.LogInformation($"Calling {nameof(GetDoctorAsync)}");

            int id = ClaimsHelper.GetUserIdFromClaims(HttpContext);

            var callback = await _doctorService.GetDoctorProfileAsync(id);

            IActionResult actionResult = callback.Match(
                doctor => Ok(doctor),
                notFound => Conflict(notFound.ResponseMessage),
                databaseException => Problem($"{LogHelper.DatabaseErrorController}{databaseException.Exception.Message}"));

            return actionResult;
        }
    }
}
