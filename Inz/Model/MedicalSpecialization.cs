using System.ComponentModel.DataAnnotations;

namespace Inz.Model
{
    public class MedicalSpecialization
    {
        public MedicalSpecialization () { }
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; } = null!;
        public ICollection<DoctorMedicalSpecialization> DoctorMedicalSpecializations { get; set; } = new List<DoctorMedicalSpecialization> ();

    }
}
