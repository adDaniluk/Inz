namespace Inz.DTOModel
{
    public class UpdatePatientDTO
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public int Phone { get; set; }
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!;
        public string PostCode { get; set; } = null!;
        public int AparmentNumber { get; set; }
    }
}
