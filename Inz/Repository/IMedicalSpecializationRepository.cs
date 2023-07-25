using Inz.Model;
using Inz.OneOfHelper;
using OneOf;

namespace Inz.Repository
{
    public interface IMedicalSpecializationRepository
    {
        public Task<OneOf<IList<MedicalSpecialization>, NotFoundResponse, DatabaseExceptionResponse>> GetMedicalSpecializationAsync(List<int> id);
    }
}
