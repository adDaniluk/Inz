using Inz.Model;

namespace Inz.DTOModel
{
    public class UpdateDoctorDTO
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public int Phone { get; set; }
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!;
        public string PostCode { get; set; } = null!;
        public int AparmentNumber { get; set; }
        public string? Biography { get; set; }
        public ICollection<MedicalSpecialization> MedicalSpecializations { get; set; } = new List<MedicalSpecialization>();
    }
}