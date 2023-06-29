using System.ComponentModel.DataAnnotations;

namespace Inz.DTOModel
{
    public class PatientDTO
    {
        [MaxLength(50)]
        public string Login { get; set; } = null!;
        [MaxLength(100)]
        public string Password { get; set; } = null!;
        [MaxLength(11)]
        [MinLength(11)]
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
        public int AparmentNumber { get; set; }
    }
}
