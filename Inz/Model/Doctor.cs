using System.ComponentModel.DataAnnotations;

namespace Inz.Model
{
    public class Doctor : User
    {
        public int Id { get; private set; }
        [Required]
        public int LicenseNumber { get; set; }
        public string? Biography { get; set; }
        [Required]
        public int AddressId { get; set; }
        public Address Address { get; set; } = null!;
        public PersonType PersonType { get; private set; } = PersonType.Doctor;
        public virtual ICollection<Calendar>? Calendars { get; set; }
        public virtual ICollection<MedicalSpecialization>? MedicalSpecializations { get; set; }
        public virtual ICollection<DoctorServices>? DoctorServices { get; set; }
        public virtual ICollection<Disease>? CuredDiseases { get; set; } 
    }
}
