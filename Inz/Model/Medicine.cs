namespace Inz.Model
{
    public class Medicine
    {
        public Medicine() { }
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public ICollection<ReceiptMedicine> ReceiptMedicines { get; set; } = new List<ReceiptMedicine>();
    }
}
