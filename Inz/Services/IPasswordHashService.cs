using Inz.Model;
using Inz.OneOfHelper;
using OneOf;

namespace Inz.Services
{
    public interface IPasswordHashService
    {
        public string GetHash(string password);
        public Task<OneOf<bool, DatabaseExceptionResponse>> ValidatePassword(string password, string login, PersonType personType);
    }
}
