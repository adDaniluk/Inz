using Inz.Model;
using Inz.OneOfHelper;
using OneOf;

namespace Inz.Repository
{
    public interface IDiseaseRepository
    {
        public Task<OneOf<IList<Disease>, DatabaseExceptionResponse>> GetDiseaseAsync(List<int> ids);
    }
}
