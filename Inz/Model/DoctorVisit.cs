using System.ComponentModel.DataAnnotations;

namespace Inz.Model
{
    public class DoctorVisit
    {
        public DoctorVisit() { }
        public int Id { get; set; }
        public int CalendarId { get; set; }
        public int? PaymentTypeId { get; set; }
        public Calendar Calendar { get; set; } = null!;
        public PaymentType? PaymentType { get; set; }
        public string? Notes { get; set; }
        [MaxLength(10)]
        public string? EndHour { get; set; }
        public int? Rating { get; set; }
        [MaxLength(200)]
        public string? RatingNote { get; set; }
        public int? AdditionalCosts { get; set; }
        public ICollection<DiseaseSuspicion> DiseaseSuspicions { get; set; } = new List<DiseaseSuspicion>();
        public ICollection<Referral> Referrals { get; set; } = new List<Referral>();
        public Receipt? Receipt { get; set; }

    }
}
