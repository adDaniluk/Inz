namespace Inz.Model
{
    public class Patient : User
    {
        public Patient() { }

        public int PatientId { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; } = null!;
        public ICollection<Calendar> Calendars { get; set; } = new List<Calendar>();
    }
}
