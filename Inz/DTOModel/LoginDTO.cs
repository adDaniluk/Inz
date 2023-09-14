using Inz.Model;

namespace Inz.DTOModel
{
    public class LoginDTO
    {
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
        public PersonType PersonType { get; set; }
    }
}
