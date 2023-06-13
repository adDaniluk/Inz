namespace Inz.Model
{
    public class TimeBlock
    {
        public TimeBlock() { }
        public int Id { get; set; }
        public string StartHour { get; set; } = null!;
        public string EndHour { get; set; } = null!;
        public ICollection<Calendar> Calendars { get; set; } = new List<Calendar>();

    }
}
