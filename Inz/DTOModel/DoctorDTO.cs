using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace Inz.DTOModel
{
    public class DoctorDTO
    {
        [MaxLength(50)]
        public string Login { get; set; } = null!;
        [MaxLength(100)]
        public string Password { get; set; } = null!;
        [MaxLength(11)]
        [MinLength(11)]
        [Required]
        public int UserId { get; set; }
        [MaxLength(100)]
        public string Email { get; set; } = null!;
        [Required]
        public int Phone { get; set; }
        [MaxLength(200)]
        public string Name { get; set; } = null!;
        [MaxLength(100)]
        public string Surname { get; set; } = null!;
        [Required]
        public DateTime DateOfBirth { get; set; }
        [MaxLength(100)]
        public string Street { get; set; } = null!;
        [MaxLength(100)]
        public string City { get; set; } = null!;
        [MaxLength(6)]
        public string PostCode { get; set; } = null!;
        [Required]
        public int AparmentNumber { get; set; }
        [Required]
        public int LicenseNumber { get; set; }
        public string? Biography { get; set; }
    }
}
