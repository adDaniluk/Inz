using Inz.DTOModel;
using Microsoft.AspNetCore.Mvc;

namespace Inz.Controllers
{
    public interface ICalendarController
    {
        public Task<IActionResult> CreateCalendarsAsync(CalendarDTO calendarDTO);
        public Task<IActionResult> GetCalendarByIdAsync(int id);
        public Task<IActionResult> GetCalendarListByDateRangeAsync(CalendarTimeframeDTO calendarTimeframeDTO);
        public Task<IActionResult> BookCalendarVisitAsync(DoctorVisitDTO doctorVisitDTO);
    }
}
