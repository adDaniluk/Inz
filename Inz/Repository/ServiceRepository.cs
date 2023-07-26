using Inz.Context;
using Inz.Model;
using Inz.OneOfHelper;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace Inz.Repository
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly DbContextApi _dbContextApi;
        public ServiceRepository(DbContextApi dbContextApi)
        {
            _dbContextApi = dbContextApi;
        }

        public async Task<OneOf<Service, NotFoundResponse, DatabaseExceptionResponse>> GetServiceAsync(int id)
        {
            try
            {
                Service? service = await _dbContextApi.Services.SingleOrDefaultAsync(x => x.Id == id);

                return service != null ? service : new NotFoundResponse();
            }
            catch (Exception exception)
            {
                return new DatabaseExceptionResponse(exception);
            }
        }
    }
}
