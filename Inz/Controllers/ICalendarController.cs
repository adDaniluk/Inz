using Inz.DTOModel;
using Microsoft.AspNetCore.Mvc;

namespace Inz.Controllers
{
    public interface ICalendarController
    {
        public Task<IActionResult> CreateCalendarAsync(CalendarDTO calendarDTO);
        public Task<IActionResult> GetCalendarAsync(int id);
        public Task<IActionResult> GetCalendarListByDateRangeAsync(DateTime startDate, DateTime endDate);
        public Task<IActionResult> BookCalendarVisitAsync(DoctorVisitDTO doctorVisitDTO);
    }
}
