using Inz.DTOModel;
using Inz.OneOfHelper;
using OneOf;

namespace Inz.Services
{
    public interface ICalendarService
    {
        public Task<OneOf<OkResponse, NotFoundResponse, DatabaseExceptionResponse>> CreateCalendarAsync(CalendarDTO calendarDTO);
    }
}
