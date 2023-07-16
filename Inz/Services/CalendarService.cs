using Inz.DTOModel;
using Inz.Model;
using Inz.OneOfHelper;
using Inz.Repository;
using OneOf;
using OneOf.Types;

namespace Inz.Services
{
    public class CalendarService : ICalendarService
    {
        private readonly ICalendarRepository _calendarRepository;
        private readonly ILogger _logger;
        public CalendarService(ICalendarRepository calendarRepository, ILogger<ICalendarRepository> logger)
        {
            _calendarRepository = calendarRepository;
            _logger = logger;

        }
        public async Task<OneOf<Calendar, NotFound, DatabaseException>> CreateCalendarAsync(CalendarDTO calendarDTO)
        {

            var returnValue = await _calendarRepository.ValidAndCreateCalendarAsync(calendarDTO);

            return returnValue.Match(
                calendar => new Calendar(),
                notFound => new NotFound(),
                databaseException => returnValue);   
        }
    }
}
