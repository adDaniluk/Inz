using Inz.DTOModel;
using Inz.DTOModel.Validators;
using Inz.Services;
using Microsoft.AspNetCore.Mvc;

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
            CalendarDTOValidator calendarDTOValidator = new CalendarDTOValidator();
            var validatorResult = calendarDTOValidator.Validate(calendarDTO);

            if(!validatorResult.IsValid) {
                return BadRequest(validatorResult.Errors.ToList().Select(x => new { Error = $"{x.ErrorCode}: {x.ErrorMessage}" }));
            }

            var returnValue = await _calendarService.CreateCalendarAsync(calendarDTO);

            var actionResult = returnValue.Match(
                calendar => Ok("New calendars have been added."),
                notFound => BadRequest($"Ids are not correct, see inner logs."),
                databaseException => Problem($"Cannot connect to the database, please contact Admin@admin.admin | " +
                    $"See inner exception: {databaseException.exception.Message}"));

            return actionResult;
        }
    }
}
