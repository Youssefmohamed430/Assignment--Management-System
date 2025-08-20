namespace Assignment__Management_System.DataLayer.DTOs
{
    public class SubmitDTO
    {
        public string FileName { get; set; }
        public int AssignmentId { get; set; }
        public string? AssignmentTitle { get; set; }
        public double grade { get; set; }
        public string? stuname { get; set; }

    }
}
