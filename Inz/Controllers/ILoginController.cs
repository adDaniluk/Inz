using Inz.DTOModel;
using Microsoft.AspNetCore.Mvc;

namespace Inz.Controllers
{
    public interface ILoginController
    {
        public Task<IActionResult> LoginAsync(LoginDTO loginDTO);
    }
}
