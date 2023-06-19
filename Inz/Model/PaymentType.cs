using System.ComponentModel.DataAnnotations;

namespace Inz.Model
{
    public class PaymentType
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string TypeName { get; set; } = null!;
        public ICollection<DoctorVisit> DoctorVisits { get; set; } = new List<DoctorVisit>();
    }
}
