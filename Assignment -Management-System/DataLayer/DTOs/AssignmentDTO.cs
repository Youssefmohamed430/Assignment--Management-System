namespace Assignment__Management_System.DataLayer.DTOs
{
    public class AssignmentDTO
    {
        public int? AssignmentId { get; set; }
        public string Title { get; set; }
        public string? CrsName { get; set; }
        public int CrsId { get; set; }
        public DateOnly DeadLine { get; set; }
    }
}
