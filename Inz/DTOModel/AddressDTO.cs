using Inz.Model;

namespace Inz.DTOModel
{
    public class AddressDTO
    {
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!;
        public string PostCode { get; set; } = null!;
        public int AparmentNumber { get; set; }
    }
}
