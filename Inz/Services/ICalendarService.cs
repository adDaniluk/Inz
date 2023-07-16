
using Inz.DTOModel;
using Inz.Model;
using Inz.OneOfHelper;
using OneOf;
using OneOf.Types;

namespace Inz.Services
{
    public interface ICalendarService
    {
        public Task<OneOf<Calendar, NotFound, DatabaseException>> CreateCalendarAsync(CalendarDTO calendarDTO);
    }
}
