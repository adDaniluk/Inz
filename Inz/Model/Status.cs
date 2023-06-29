using System.ComponentModel.DataAnnotations;

namespace Inz.Model
{
    public class Status
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string StatusName { get; set; } = null!;
        public virtual ICollection<Calendar> Calendars { get; set; } = new List<Calendar>();
    }
}
