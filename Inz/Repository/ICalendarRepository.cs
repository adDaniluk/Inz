using Inz.DTOModel;
using Inz.Model;
using Inz.OneOfHelper;
using OneOf;
using OneOf.Types;

namespace Inz.Repository
{
    public interface ICalendarRepository
    {
        public Task<OneOf<Calendar, NotFound, DatabaseException>> ValidAndCreateCalendarAsync(CalendarDTO calendarDTO);
    }
}
