namespace Inz.Model
{
    public class DoctorServices
    {
        public int DoctorId { get; set; }
        public int ServiceId { get; set; }
        public int Price { get; set; }
        public Doctor Doctor { get; set; } = null!;
        public Service Service { get; set; } = null!;
    }
}
