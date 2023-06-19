
using System.ComponentModel.DataAnnotations;

namespace Inz.Model
{
    public abstract class User : TimestampModel
    {
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
    }
}
