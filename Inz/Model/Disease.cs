using System.ComponentModel.DataAnnotations;

namespace Inz.Model
{
    public class Disease
    {
        public int Id { get; set; }
        [MaxLength(200)]
        public string Name { get; set; } = null!;
        public virtual ICollection<Doctor>? Doctors { get; set; }
        public virtual ICollection<DiseaseSuspicion> DiseaseSuspicions { get; set; } = new List<DiseaseSuspicion>();
    }
}
