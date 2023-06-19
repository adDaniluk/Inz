using System.ComponentModel.DataAnnotations;

namespace Inz.Model
{
    public class Service
    {
        public int Id { get; set; }
        public string Description { get; set; } = null!;
        public string Name { get; set; } = null!;
        public ICollection<Calendar> Calendars { get; set; } = new List<Calendar>();
        public ICollection<DoctorService> DoctorServices { get; set; } = new List<DoctorService>();
    }
}
