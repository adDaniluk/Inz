﻿using Inz.DTOModel;
using Inz.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inz.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CalendarController : ControllerBase, ICalendarController
    {
        private readonly ICalendarService _calendarService;
        private readonly IDoctorVisitService _doctorVisitService;
        private readonly ILogger _logger;

        public const string dbErrorInformation = "Cannot connect to the database, please contact Admin@admin.admin. See inner exception:";

        public CalendarController(ICalendarService calendarService,
            IDoctorVisitService doctorVisitService,
            ILogger<ICalendarController> logger)
        {
            _calendarService = calendarService;
            _doctorVisitService = doctorVisitService;
            _logger = logger;
        }

        [Route("AddCalendar")]
        [Authorize(Roles = "Doctor")]
        [HttpPost]
        public async Task<IActionResult> CreateCalendarAsync(CalendarDTO calendarDTO)
        {
            _logger.LogInformation(message: $"Calling {nameof(CreateCalendarAsync)}");

            var callback = await _calendarService.InsertCalendarAsync(calendarDTO);

            var actionResult = callback.Match(
                calendar => Ok(calendar.ResponseMessage),
                notFound => NotFound(notFound.ResponseMessage),
                databaseException => Problem($"{dbErrorInformation}: {databaseException.Exception.Message}"));

            return actionResult;
        }

        [Route("GetCalendar")]
        [HttpGet]
        public async Task<IActionResult> GetCalendarAsync(int id)
        { 
            _logger.LogInformation(message: $"Calling {nameof(GetCalendarAsync)}");

            if(id < 0)
                return NotFound($"{id} cannot be negative");

            var callback = await _calendarService.GetCalendarByIdAsync(id);

            var actionResult = callback.Match(
                calendar => Ok(calendar),
                notFound => NotFound(notFound.ResponseMessage),
                databaseException => Problem($"{dbErrorInformation}: {databaseException.Exception.Message}"));

            return actionResult;
        }

        [Route("GetCalendars")]
        [HttpGet]
        public async Task<IActionResult> GetCalendarListByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            _logger.LogInformation(message: $"Calling {nameof(GetCalendarListByDateRangeAsync)}");

            var callback = await _calendarService.GetCalendarListByDateRangeAsync(startDate, endDate);

            var actionResult = callback.Match(
                calendar => Ok(calendar),
                notFound => NotFound(notFound.ResponseMessage),
                databaseException => Problem($"{dbErrorInformation}: {databaseException.Exception.Message}"));

            return actionResult;
        }

        
        [Route("BookCalendar")]
        [HttpPut]
        [Authorize(Roles ="Patient")]
        public async Task<IActionResult> BookCalendarVisitAsync(DoctorVisitDTO doctorVisitDTO)
        {
            _logger.LogInformation(message: $"Calling {nameof(BookCalendarVisitAsync)}");

            var callback = await _doctorVisitService.BookDoctorVisitAsync(doctorVisitDTO);

            var actionResult = callback.Match(
                calendar => Ok(calendar),
                notFound => NotFound(notFound.ResponseMessage),
                databaseException => Problem($"{dbErrorInformation}: {databaseException.Exception.Message}"));

            return actionResult;
        }
    }
}
