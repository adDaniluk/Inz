using Inz.Context;
using Inz.Model;
using Inz.OneOfHelper;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace Inz.Repository
{
    public class StatusRepository : IStatusRepository
    {
        private readonly DbContextApi _dbContextApi;

        public StatusRepository(DbContextApi dbContextApi)
        {
            _dbContextApi = dbContextApi;
        }

        public async Task<OneOf<Status?, DatabaseExceptionResponse>> GetStatusAsync(StatusEnum statusEnum)
        {
            try
            {
                Status? status = await _dbContextApi.Statuses.SingleOrDefaultAsync(x => x.CalendarStatus == statusEnum);
                return status;
            }
            catch(Exception exception)
            {
                return new DatabaseExceptionResponse(exception);
            }
        }
    }
}
