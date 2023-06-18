
using System.ComponentModel.DataAnnotations;

namespace Inz.Model
{
    public abstract class User : TimestampModel
    {
        [MaxLength(50)]
        public string Login { get; set; } = null!;
        [MaxLength(100)]
        public string Password { get; set; } = null!;
        [MaxLength(100)]
        public string Email { get; set; } = null!;
        [MaxLength(100)]
        public string Phone { get; set; } = null!;
        [MaxLength(200)]
        public string Name { get; set; } = null!;
        [MaxLength(200)]
        public string Surname { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }

    }
}
