using Inz.Model;
using Inz.OneOfHelper;
using OneOf;

namespace Inz.Repository
{
    public interface IServiceRepository
    {
        public Task<OneOf<Service?, DatabaseExceptionResponse>> GetServiceAsync(int id);
    }
}
