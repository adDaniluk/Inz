using Inz.DTOModel;
using Inz.OneOfHelper;
using Inz.Repository;
using OneOf;

namespace Inz.Services
{
    public class CalendarService : ICalendarService
    {
        private readonly ICalendarRepository _calendarRepository;
        public CalendarService(ICalendarRepository calendarRepository)
        {
            _calendarRepository = calendarRepository;
        }
        public async Task<OneOf<OkResponse, NotFoundResponse, DatabaseExceptionResponse>> CreateCalendarAsync(CalendarDTO calendarDTO)
        {
            var returnValue = await _calendarRepository.ValidAndCreateCalendarAsync(calendarDTO);

            return returnValue.Match(
                okResponse => okResponse,
                notFound => notFound,
                databaseException => returnValue);   
        }
    }
}
