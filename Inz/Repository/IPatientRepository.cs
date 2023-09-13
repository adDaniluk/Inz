using Inz.Model;
using Inz.OneOfHelper;
using OneOf;

namespace Inz.Repository
{
    public interface IPatientRepository
    {
        public Task<OneOf<OkResponse, DatabaseExceptionResponse>> InsertPatientAsync(Patient patient);
        public Task<OneOf<OkResponse, DatabaseExceptionResponse>> UpdatePatientAsyc(Patient patient);
        public Task<OneOf<Patient, NotFoundResponse, DatabaseExceptionResponse>> GetPatientAsync(int id);
        public Task<OneOf<bool, DatabaseExceptionResponse>> CheckExistingLoginAsync(string login);
        public Task<OneOf<string, DatabaseExceptionResponse>> GetPasswordAsync(string login);
    }
}
