using System.ComponentModel.DataAnnotations;

namespace Inz.Model
{
    public class Service
    {
        public int Id { get; set; }
        [MaxLength(200)]
        public string Description { get; set; } = null!;
        [MaxLength(200)]
        public string Name { get; set; } = null!;
        public virtual ICollection<Calendar> Calendars { get; set; } = new List<Calendar>();
        public virtual ICollection<DoctorService> DoctorServices { get; set; } = new List<DoctorService>();
    }
}
