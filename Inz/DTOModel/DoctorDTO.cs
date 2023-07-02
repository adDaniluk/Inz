namespace Inz.DTOModel
{
    public class DoctorDTO
    {
        public string? Login { get; set; }
        public string? Password { get; set; }
        public string? UserId { get; set; }
        public string? Email { get; set; } 
        public int Phone { get; set; }
        public string? Name { get; set; } 
        public string? Surname { get; set; } 
        public DateTime DateOfBirth { get; set; }
        public string? Street { get; set; } 
        public string? City { get; set; } 
        public string? PostCode { get; set; } 
        public int AparmentNumber { get; set; }
        public int LicenseNumber { get; set; }
        public string? Biography { get; set; }
    }
}
