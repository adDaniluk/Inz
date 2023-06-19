namespace Inz.Model
{
    public class DoctorMedicalSpecialization
    {
        public int DoctorId { get; set; }
        public int MedicalSpecializationId { get; set; }
        public Doctor Doctor { get; set; } = null!;
        public MedicalSpecialization MedicalSpecialization { get; set; } = null!;
    }
}
