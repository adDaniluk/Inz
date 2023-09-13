namespace Inz.Model
{
    public class Patient : User
    {
        public int Id { get; private set; }
        public int AddressId { get; set; }
        public PersonType PersonType { get; private set; } = PersonType.Patient;
        public Address Address { get; set; } = null!;
        public virtual ICollection<Calendar> Calendars { get; set; } = new List<Calendar>();
    }
}
