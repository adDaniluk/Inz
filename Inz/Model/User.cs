using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inz.Model
{
    public abstract class User : TimestampModel
    {
        [MaxLength(200)]
        public string Name { get; set; } = null!;
        [MaxLength(200)]
        public string Surname { get; set; } = null!;
        [MaxLength(50)]
        public string Login { get; set; } = null!;
        [MaxLength(100)]
        public string Password { get; set; } = null!;
        [MaxLength(11)]
        [MinLength(11)]
        public string UserId { get; set; } = null!;
        [MaxLength(100)]
        public string Email { get; set; } = null!;
        [Required]
        public int Phone { get; set; }
        [Required]
        [Column(TypeName = "Date")]
        public DateTime DateOfBirth { get; set; }
    }
}
