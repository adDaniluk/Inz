namespace Inz.DTOModel
{
    public class UpdateDoctorDTO
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public int Phone { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? PostCode { get; set; }
        public int AparmentNumber { get; set; }
        public string? Biography { get; set; }
    }
}