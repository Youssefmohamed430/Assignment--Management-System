namespace Assignment__Management_System.Models.Entities
{
    public class Assignment
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int CrsId { get; set; }
        public DateOnly DeadLine { get; set; }
        public Course? course { get; set; }
        public List<Submission>? Submissions { get; set; }
    }
}
