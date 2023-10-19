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

        public CalendarRepository(DbContextApi dbContextApi)
        {
            _dbContextApi = dbContextApi;

        }

        public async Task<OneOf<OkResponse, DatabaseExceptionResponse>> InsertCalendarAsync(List<Calendar> calendars)
        {
            try
            { 
                await _dbContextApi.Calendars.AddRangeAsync(calendars);
                await _dbContextApi.SaveChangesAsync();
                return new OkResponse();
            }
            catch (Exception exception)
            {
                return new DatabaseExceptionResponse(exception);
            }
        }

        public async Task<OneOf<Calendar?, DatabaseExceptionResponse>> GetCalendarAsync(int id)
        {
            try
            {
                Calendar? calendar = await _dbContextApi.Calendars.
                    Include(x => x.Doctor).
                    Include(x => x.Status).
                    Include(x => x.Doctor).
                    Include(x => x.DoctorVisit).
                    Include(x => x.Patient).
                    SingleOrDefaultAsync(x => x.Id == id && x.IsDeleted == 0);

                return calendar;
            }
            catch(Exception exception)
            {
                return new DatabaseExceptionResponse(exception);
            }
        }

        public async Task<OneOf<List<Calendar>, DatabaseExceptionResponse>> GetCalendarsByDoctorIdAsync(int doctorId)
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

        public async Task<OneOf<OkResponse, DatabaseExceptionResponse>> UpdateCalendarAsync(Calendar calendar)
        {
            try
            {
                _dbContextApi.Calendars.Update(calendar);
                await _dbContextApi.SaveChangesAsync();
                return new OkResponse();
            }
            catch(Exception exception)
            {
                return new DatabaseExceptionResponse(exception);
            }
        }
    }
}
