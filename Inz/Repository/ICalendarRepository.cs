using Inz.DTOModel;
using Inz.OneOfHelper;
using OneOf;

namespace Inz.Repository
{
    public interface ICalendarRepository
    {
        public Task<OneOf<OkResponse, NotFoundResponse, DatabaseExceptionResponse>> ValidAndCreateCalendarAsync(CalendarDTO calendarDTO);
    }
}
