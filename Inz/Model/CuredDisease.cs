namespace Inz.Model
{
    public class CuredDisease
    {
        public int DoctorId { get; set; }
        public int DiseaseId { get; set; }
        public Doctor Doctor { get; set; } = null!;
        public Disease Disease { get; set; } = null!;
    }
}
