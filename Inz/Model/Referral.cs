using System.Diagnostics.Contracts;

namespace Inz.Model
{
    public class Referral
    {
        public int Id { get; set; }
        public string Description { get; set; } = null!;
        public int DoctorId { get; set; }
        public DoctorVisit DoctorVisit { get; set; } = null!;
    }
}
