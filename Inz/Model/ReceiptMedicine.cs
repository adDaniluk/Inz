namespace Inz.Model
{
    public class ReceiptMedicine
    {
        public ReceiptMedicine() { }
        public int Id { get; set; }
        public int Quantity { get; set; }
        public Receipt Receipt { get; set; } = null!;
        public Medicine Medicine { get; set; } = null!;
    }
}
