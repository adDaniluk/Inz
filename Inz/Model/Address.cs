using System.ComponentModel.DataAnnotations;

namespace Inz.Model
{
    public class Address : TimestampModel
    {
        public Address() { }

        public int Id { get; set; }
        [MaxLength(100)]
        public string Street { get; set; } = null!;
        [MaxLength(100)]
        public string City { get; set; } = null!;
        [MaxLength(6)]
        public string PostCode { get; set; } = null!;
        public int AparmentNumber { get; set; }
        public Patient? Patient { get; set; }
        public Doctor? Doctor { get; set; }
    }
}
