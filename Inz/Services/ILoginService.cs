using Inz.DTOModel;
using Inz.OneOfHelper;
using OneOf;

namespace Inz.Services
{
    public interface ILoginService
    {
        public Task<OneOf<OkResponse, NotAutorizedResponse, DatabaseExceptionResponse>> SignIn(LoginDTO loginDTO);
    }
}
