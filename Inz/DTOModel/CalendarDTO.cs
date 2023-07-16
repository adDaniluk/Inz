namespace Inz.DTOModel
{
    public class CalendarDTO
    {
        public int DoctorId { get; set; }
        public ICollection<int> TimeBlockIds { get; set; } = null!;
        public DateTime Date { get; set; }
    }
}
