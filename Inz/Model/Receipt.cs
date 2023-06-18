using System.ComponentModel.DataAnnotations;

namespace Inz.Model
{
    public class Receipt
    {
        public Receipt() { }
        public int Id { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Description { get; set; } = null!;
        public ICollection<ReceiptMedicine> ReceiptMedicines { get; set; } = new List<ReceiptMedicine>();
        public DoctorVisit DoctorVisit { get; set; } = null!;
        public int DoctorVisitId { get; set; }

    }
}
