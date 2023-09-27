using Inz.Model;
using Inz.OneOfHelper;
using OneOf;

namespace Inz.Repository
{
    public interface IDoctorRepository
    {
        public Task<OneOf<OkResponse, DatabaseExceptionResponse>> InsertDoctorAsync(Doctor doctor);
        public Task<OneOf<Doctor?, DatabaseExceptionResponse>> GetDoctorByIdAsync(int id);
        public Task<OneOf<string, DatabaseExceptionResponse>> GetPasswordAsync(string login);
        public Task<OneOf<OkResponse, DatabaseExceptionResponse>> UpdateDoctorAsync(Doctor doctor);
        public Task<OneOf<bool, DatabaseExceptionResponse>> CheckExistingLoginAsync(string login);
    }
}
