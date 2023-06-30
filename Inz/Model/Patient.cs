namespace Inz.Model
{
    public class Patient : User
    {
        public int Id { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; } = null!;
        public virtual ICollection<Calendar> Calendars { get; set; } = new List<Calendar>();
    }
}
