using System.ComponentModel.DataAnnotations;

namespace Inz.Model
{
    public class Medicine
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public ICollection<ReceiptMedicine> ReceiptMedicines { get; set; } = new List<ReceiptMedicine>();
    }
}
