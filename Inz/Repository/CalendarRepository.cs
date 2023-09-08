using Inz.Context;
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

        public async Task<OneOf<OkResponse, DatabaseExceptionResponse>> InsertCalendar(List<Calendar> calendars)
        {
            try
            { 
                await _dbContextApi.Calendars.AddRangeAsync(calendars);
                await _dbContextApi.SaveChangesAsync();
                return new OkResponse();
            }
            catch (Exception exception) {
                _logger.LogError(message: $"{exception.Message}");
                return new DatabaseExceptionResponse(exception);
            }
        }

        public async Task<OneOf<Calendar, NotFoundResponse, DatabaseExceptionResponse>> GetCalendar(int id)
        {
            try
            {
                Calendar? calendar = await _dbContextApi.Calendars.
                    Where(x => x.Id == id && x.IsDeleted == 0).
                    Include(x => x.Doctor).
                    Include(x => x.Status).
                    Include(x => x.Doctor).
                    Include(x => x.DoctorVisit).
                    SingleOrDefaultAsync();

                return calendar != null ? calendar : new NotFoundResponse();
            }
            catch(Exception exception)
            {
                return new DatabaseExceptionResponse(exception);
            }
        }

        public async Task<OneOf<List<Calendar>, DatabaseExceptionResponse>> GetCalendarsByDoctorId(int doctorId)
        {
            try
            {
                List<Calendar> calendars = await _dbContextApi.Calendars
                    .Where(x => x.DoctorId == doctorId && x.IsDeleted == 0)
                    .ToListAsync();
                
                return calendars;
            }
            catch(Exception exception)
            {
                return new DatabaseExceptionResponse(exception);
            }
        }

        public async Task<OneOf<List<Calendar>, DatabaseExceptionResponse>> GetCalendarListByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                List<Calendar> calendars = await _dbContextApi.Calendars
                    .Where(x => x.Date.Date <= endDate && x.Date.Date >= startDate && x.IsDeleted == 0)
                    .ToListAsync();

                return calendars;
            }
            catch(Exception exception)
            {
                return new DatabaseExceptionResponse(exception);
            }
        }
    }
}
