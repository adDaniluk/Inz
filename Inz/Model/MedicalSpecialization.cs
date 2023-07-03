using System.ComponentModel.DataAnnotations;

namespace Inz.Model
{
    public class MedicalSpecialization
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; } = null!;
        public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor> ();
    }
}
