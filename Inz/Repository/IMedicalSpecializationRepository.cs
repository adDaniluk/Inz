using Inz.Model;
using Inz.OneOfHelper;
using OneOf;

namespace Inz.Repository
{
    public interface IMedicalSpecializationRepository
    {
        public Task<OneOf<IList<MedicalSpecialization>, DatabaseExceptionResponse>> GetMedicalSpecializationAsync(List<int> id);
    }
}
