namespace Inz.Model
{
    public class DoctorService
    {
        public DoctorService() { }
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public int ServiceId { get; set; }
        public Doctor Doctor { get; set; } = null!;
        public Service Service { get; set; } = null!;

    }
}
