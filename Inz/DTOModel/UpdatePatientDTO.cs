using System.ComponentModel.DataAnnotations;

namespace Inz.DTOModel
{
    public class UpdatePatientDTO
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Email { get; set; } = null!;
        public int Phone { get; set; }
        [MaxLength(100)]
        public string Street { get; set; } = null!;
        [MaxLength(100)]
        public string City { get; set; } = null!;
        [MaxLength(6)]
        public string PostCode { get; set; } = null!;
        public int AparmentNumber { get; set; }
    }
}
