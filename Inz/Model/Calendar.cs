namespace Inz.Model
{
    public class Calendar : TimestampModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int? ServicePrice { get; set; }
        public int DoctorId { get; set; }
        public int? PatientId { get; set; }
        public int TimeBlockId { get; set; }
        public int? ServiceId { get; set; }
        public int StatusId { get; set; }
        public DoctorVisit? DoctorVisit { get; set; }
        public Doctor Doctor { get; set; } = null!;
        public Patient? Patient { get; set; }
        public Status Status { get; set; } = null!;
        public TimeBlock TimeBlock { get; set; } = null!;
        public Service? Service { get; set; }
    }
}
