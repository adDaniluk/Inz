using Inz.DTOModel;
using Inz.Model;
using Inz.OneOfHelper;
using OneOf;

namespace Inz.Services
{
    public interface ICalendarService
    {
        public Task<OneOf<OkResponse, NotFoundResponse, DatabaseExceptionResponse>> InsertCalendarAsync(CalendarDTO calendarDTO);
        public Task<OneOf<Calendar, NotFoundResponse, DatabaseExceptionResponse>> GetCalendarByIdAsync(int id);
        public Task<OneOf<List<Calendar>, NotFoundResponse, DatabaseExceptionResponse>> GetCalendarListByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
