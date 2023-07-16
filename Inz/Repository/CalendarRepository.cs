using Inz.Context;
using Inz.DTOModel;
using Inz.Model;
using Inz.OneOfHelper;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;

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
        public async Task<OneOf<Calendar, NotFound, DatabaseException>> ValidAndCreateCalendarAsync(CalendarDTO calendarDTO)
        {
            try
            {
                bool doesDoctorExist = await _dbContextApi.Doctors.AnyAsync(x => x.Id == calendarDTO.DoctorId);

                bool doesDoctorHaveCalendarAlready = await _dbContextApi.Calendars
                    .AnyAsync(x => x.DoctorId == calendarDTO.DoctorId
                            && x.Date == calendarDTO.Date
                            && !calendarDTO.TimeBlockIds.Contains(x.TimeBlockId));

                if (!doesDoctorExist || doesDoctorHaveCalendarAlready)
                {
                    if (!doesDoctorExist)
                        _logger.LogError(message: $"Doctor with Id:{calendarDTO.DoctorId} does not exist.");
                    else
                        _logger.LogError(message: $"Doctor has already created calendar blocks.");

                    return new NotFound();
                }

                List<Calendar> calendars = CastCalendarDTOIntoCalendar(calendarDTO);

                await _dbContextApi.Calendars.AddRangeAsync(calendars);

                await _dbContextApi.SaveChangesAsync();

                return new Calendar();

            }
            catch (Exception exception) {
                _logger.LogError(message: $"{exception.Message}");
                return new DatabaseException(exception);
            }
        }

        private List<Calendar> CastCalendarDTOIntoCalendar(CalendarDTO calendarDTO)
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
