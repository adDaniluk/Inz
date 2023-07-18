using Inz.Context;
using Inz.DTOModel;
using Inz.Model;
using Inz.OneOfHelper;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace Inz.Repository
{
    public class CalendarRepository : ICalendarRepository
    {

        private readonly DbContextApi _dbContextApi;
        private readonly ILogger _logger;

        public CalendarRepository(DbContextApi dbContextApi, ILogger<ICalendarRepository> logger)
        {
            _dbContextApi = dbContextApi;
            _logger = logger;

        }
        public async Task<OneOf<OkResponse, NotFoundResponse, DatabaseExceptionResponse>> ValidAndCreateCalendarAsync(CalendarDTO calendarDTO)
        {
            try
            {
                string log;

                bool doesDoctorExist = await _dbContextApi.Doctors.AnyAsync(x => x.Id == calendarDTO.DoctorId);

                bool doesDoctorHaveCalendarAlready = await _dbContextApi.Calendars
                    .AnyAsync(x => x.DoctorId == calendarDTO.DoctorId
                            && x.Date == calendarDTO.Date
                            && !calendarDTO.TimeBlockIds.Contains(x.TimeBlockId));

                if (!doesDoctorExist || doesDoctorHaveCalendarAlready)
                {
                    log = !doesDoctorExist ?
                        $"Doctor with Id:{calendarDTO.DoctorId} does not exist." :
                        $"Calendar cannot be created for the same date/block.";

                    _logger.LogInformation(message: log);
                    return new NotFoundResponse(log);
                }

                List<Calendar> calendars = CastCalendarDTOIntoCalendar(calendarDTO);

                await _dbContextApi.Calendars.AddRangeAsync(calendars);

                await _dbContextApi.SaveChangesAsync();

                log = "New calendar(s) have been added";
                _logger.LogInformation(message: log);
                return new OkResponse(log);

            }
            catch (Exception exception) {
                _logger.LogError(message: $"{exception.Message}");
                return new DatabaseExceptionResponse(exception);
            }
        }
        private static List<Calendar> CastCalendarDTOIntoCalendar(CalendarDTO calendarDTO)
        {
            List<Calendar> calendars = calendarDTO.TimeBlockIds.Select(id => new Calendar
            {
                Date = calendarDTO.Date,
                DoctorId = calendarDTO.DoctorId,
                TimeBlockId = id,
                Timestamp = DateTime.Now,
                AlterTimestamp = DateTime.Now
            }).ToList();

            return calendars;
        }
    }
}
