using Inz.DTOModel;
using Inz.Model;
using Inz.OneOfHelper;
using OneOf;

namespace Inz.Repository
{
    public interface IPatientRepository
    {
        public Task InsertPatientAsync(Patient patient);
        public Task<OneOf<OkResponse, DatabaseExceptionResponse>> SaveChangesAsync();
        public Task<OneOf<OkResponse, NotFoundResponse, DatabaseExceptionResponse>> UpdatePatientAsyc(UpdatePatientDTO updatePatientDTO);
    }
}
