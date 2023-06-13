namespace Inz.Model
{
    public class Status
    {
        public Status() { }

        public int Id { get; set; }
        public string StatusName { get; set; } = null!;
        public ICollection<Calendar> Calendars { get; set; } = new List<Calendar>();

    }
}
