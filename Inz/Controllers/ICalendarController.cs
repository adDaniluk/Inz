using Inz.DTOModel;
using Microsoft.AspNetCore.Mvc;

namespace Inz.Controllers
{
    public interface ICalendarController
    {
        public Task<IActionResult> CreateCalendarAsync(CalendarDTO calendarDTO);
    }
}
