namespace Inz.Model
{
    public class Doctor : User
    {
        public Doctor() { }
        public int DoctorId { get; set; }
        public int LicenseNumber { get; set; }
        public string? Biography { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; } = null!;
        public ICollection<Calendar> Calendars { get; set;} = new List<Calendar>();
        public ICollection<DoctorService> DoctorServices { get; set; } = new List<DoctorService>();
        public ICollection<DoctorMedicalSpecialization> DoctorMedicalSpecializations = new List<DoctorMedicalSpecialization>();
    }
}
