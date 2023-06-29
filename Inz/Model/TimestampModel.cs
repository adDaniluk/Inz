using System.ComponentModel.DataAnnotations;

namespace Inz.Model
{
    public abstract class TimestampModel
    {
        public DateTime Timestamp { get; set; } 
        public DateTime AlterTimestamp { get; set; }
        public DateTime? DeleteTimestamp { get; set; }
        [Required]
        public int IsDeleted { get; set; } = 0;
    }
}
