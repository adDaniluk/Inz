namespace Inz.Model
{
    public class MedicalSpecialization
    {
        public MedicalSpecialization () { }
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<DoctorMedicalSpecialization> DoctorMedicalSpecializations { get; set; } = new List<DoctorMedicalSpecialization> ();

    }
}
