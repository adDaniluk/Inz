namespace Inz.Model
{
    public class DiseaseSuspicion
    {
        public DiseaseSuspicion() { }
        public int DoctorVisitId { get; set; }
        public int DiseaseId { get; set; }
        public DoctorVisit DoctorVisit { get; set; } = null!;
        public Disease Disease { get; set; } = null!;
    }
}
