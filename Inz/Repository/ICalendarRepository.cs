using Inz.DTOModel;
using Inz.Model;
using Inz.OneOfHelper;
using OneOf;

namespace Inz.Repository
{
    public interface ICalendarRepository
    {
        public Task<OneOf<OkResponse, DatabaseExceptionResponse>> InsertCalendarAsync(List<Calendar> calendar);
        public Task<OneOf<Calendar?, DatabaseExceptionResponse>> GetCalendarAsync(int calendarId);
        public Task<OneOf<List<Calendar>, DatabaseExceptionResponse>> GetCalendarsByDoctorIdAsync(int doctorId);
        public Task<OneOf<List<Calendar>, DatabaseExceptionResponse>> GetCalendarListByDateRangeAsync(DateTime startDate, DateTime endDate);
        public Task<OneOf<OkResponse, DatabaseExceptionResponse>> UpdateCalendarAsync(Calendar calendar);
    }
}
