using System.ComponentModel.DataAnnotations;

namespace Inz.Model
{
    public class Doctor : User
    {
        public int Id { get; set; }
        [Required]
        public int LicenseNumber { get; set; }
        public string? Biography { get; set; }
        [Required]
        public int AddressId { get; set; }
        public Address Address { get; set; } = null!;
        public virtual ICollection<Calendar> Calendars { get; set;} = new List<Calendar>();
        public virtual ICollection<DoctorMedicalSpecialization> DoctorMedicalSpecializations { get; set; } = new List<DoctorMedicalSpecialization>();
        public virtual ICollection<DoctorService> DoctorServices { get; set; } = new List<DoctorService>();
        public virtual ICollection<CuredDisease> CuredDiseases { get; set; } = new List<CuredDisease>();
    }
}
