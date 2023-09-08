using Inz.DTOModel;
using Inz.Services;
using Microsoft.AspNetCore.Mvc;
using OneOf.Types;

namespace Inz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarController : ControllerBase, ICalendarController
    {
        private readonly ICalendarService _calendarService;
        private readonly ILogger _logger;
        public CalendarController(ICalendarService calendarService, ILogger<ICalendarController> logger)
        {
            _calendarService = calendarService;
            _logger = logger;
        }

        [Route("AddCalendar")]
        [HttpPost]
        public async Task<IActionResult> CreateCalendarAsync(CalendarDTO calendarDTO)
        {
            _logger.LogInformation($"Calling {nameof(CreateCalendarAsync)}");

            var callback = await _calendarService.InsertCalendarAsync(calendarDTO);

            var actionResult = callback.Match(
                calendar => Ok(calendar.ResponseMessage),
                notFound => NotFound(notFound.ResponseMessage),
                databaseException => Problem($"Cannot connect to the database, please contact Admin@admin.admin | " +
                    $"See inner exception: {databaseException.Exception.Message}"));

            return actionResult;
        }

        [Route("GetCalendar")]
        [HttpGet]
        public async Task<IActionResult> GetCalendarAsync(int id)
        { 
            _logger.LogInformation($"Calling {nameof(GetCalendarAsync)}");

            if(id < 0)
                return NotFound($"{id} cannot be negative");

            var callback = await _calendarService.GetCalendarByIdAsync(id);

            var actionResult = callback.Match(
                calendar => Ok(calendar),
                notFound => NotFound(notFound.ResponseMessage),
                databaseException => Problem($"Cannot connect to the database, please contact Admin@admin.admin | " +
                    $"See inner exception: {databaseException.Exception.Message}"));

            return actionResult;
        }

        [Route("GetCalendars")]
        [HttpGet]
        public async Task<IActionResult> GetCalendarListByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            _logger.LogInformation($"Calling {nameof(GetCalendarListByDateRangeAsync)}");

            var callback = await _calendarService.GetCalendarListByDateRangeAsync(startDate, endDate);

            var actionResult = callback.Match(
                calendar => Ok(calendar),
                notFound => NotFound(notFound.ResponseMessage),
                databaseException => Problem($"Cannot connect to the database, please contact Admin@admin.admin | " +
                    $"See inner exception: {databaseException.Exception.Message}"));

            return actionResult;
        }
    }
}
