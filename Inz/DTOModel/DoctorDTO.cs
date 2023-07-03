namespace Inz.DTOModel
{
    public class DoctorDTO
    {
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int Phone { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!;
        public string PostCode { get; set; } = null!;
        public int AparmentNumber { get; set; }
        public int LicenseNumber { get; set; }
        public string? Biography { get; set; }
    }
}
