using Inz.Model;

namespace Inz.DTOModel
{
    public class PatientDTO
    {
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public int AddressId { get; set; }
    }
}
