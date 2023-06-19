namespace Inz.Model
{
    public class ReceiptMedicine
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public Receipt Receipt { get; set; } = null!;
        public int ReceiptId { get; set; }
        public Medicine Medicine { get; set; } = null!;
        public int MedicineId { get; set; }
    }
}
