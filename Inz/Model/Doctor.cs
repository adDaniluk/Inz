namespace Inz.Model
{
    public class Doctor : User
    {
        public int DoctorId { get; set; }
        public int LicenseNumber { get; set; }
        public string? Biography { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; } = null!;
        public ICollection<Calendar> Calendars { get; set;} = new List<Calendar>();
        public ICollection<DoctorMedicalSpecialization> DoctorMedicalSpecializations { get; set; } = new List<DoctorMedicalSpecialization>();
        public ICollection<DoctorService> DoctorServices { get; set; } = new List<DoctorService>();
        public ICollection<CuredDisease> CuredDiseases { get; set; } = new List<CuredDisease>();
    }
}
