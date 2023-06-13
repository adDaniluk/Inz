namespace Inz.Model
{
    public abstract class TimestampModel
    {
        public DateTime Timestamp { get; set; } 
        public DateTime AlterTimestamp { get; set; }
        public DateTime? DeleteTimestamp { get; set; }
        public int IsDeleted { get; set; } = 0;
    }
}
