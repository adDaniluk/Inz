namespace Inz.Model
{
    public class PaymentType
    {
        public PaymentType() { }
        public int Id { get; set; }
        public string TypeName { get; set; } = null!;
        public ICollection<DoctorVisit> DoctorVisits { get; set; } = new List<DoctorVisit>();
    }
}
