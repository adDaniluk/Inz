using System.ComponentModel.DataAnnotations;

namespace Inz.Model
{
    public class Address : TimestampModel
    {
        public int Id { get; set; }
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!;
        public string PostCode { get; set; } = null!;
        public int AparmentNumber { get; set; }
        public Patient? Patient { get; set; }
        public Doctor? Doctor { get; set; }
    }
}
