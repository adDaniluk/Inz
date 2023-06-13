namespace Inz.Model
{
    public class DoctorVisit
    {
        public DoctorVisit() { }
        public int Id { get; set; }
        public int CalendarId { get; set; }
        public int PaymentTypeId { get; set; }
        public Calendar Calendar { get; set; } = null!;
        public PaymentType PaymentType { get; set; } = null!;
        public string? Notes { get; set; }
        public string? EndHour { get; set; }
        public int Rating { get; set; }
        public string? RatingNote { get; set; }
        public int AdditionalCosts { get; set; }
        public ICollection<DiseaseSuspicion> DiseaseSuspicions { get; set; } = new List<DiseaseSuspicion>();
        public Receipt? Receipt { get; set; }

    }
}
