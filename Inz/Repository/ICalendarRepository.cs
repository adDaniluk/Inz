using Inz.DTOModel;
using Inz.Model;
using Inz.OneOfHelper;
using OneOf;

namespace Inz.Repository
{
    public interface ICalendarRepository
    {
        public Task<OneOf<OkResponse, DatabaseExceptionResponse>> InsertCalendar(List<Calendar> calendar);
        public Task<OneOf<Calendar, NotFoundResponse, DatabaseExceptionResponse>> GetCalendar(int id);
        public Task<OneOf<List<Calendar>, DatabaseExceptionResponse>> GetCalendarsByDoctorId(int doctorId);
        public Task<OneOf<List<Calendar>, DatabaseExceptionResponse>> GetCalendarListByDateRangeAsync(DateTime startDate, DateTime endDate);

    }
}
