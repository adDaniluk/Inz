namespace Inz.Model
{
    public class Address : TimestampModel
    {
        public Address() { }

        public int Id { get; set; }
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!;
        public int PostCode { get; set; }
        public Patient? Patient { get; set; }
        public Doctor? Doctor { get; set; }
    }
}
