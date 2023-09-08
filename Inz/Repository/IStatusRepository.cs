using Inz.Model;
using Inz.OneOfHelper;
using OneOf;

namespace Inz.Repository
{
    public interface IStatusRepository
    {
        public Task<OneOf<Status, NotFoundResponse, DatabaseExceptionResponse>> GetStatus(StatusEnum statusEnum);
    }
}
