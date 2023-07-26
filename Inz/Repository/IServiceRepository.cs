using Inz.Model;
using Inz.OneOfHelper;
using OneOf;

namespace Inz.Repository
{
    public interface IServiceRepository
    {
        public Task<OneOf<Service, NotFoundResponse, DatabaseExceptionResponse>> GetServiceAsync(int id);
    }
}
