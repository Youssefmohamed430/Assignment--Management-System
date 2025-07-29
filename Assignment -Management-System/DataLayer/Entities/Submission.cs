namespace Assignment__Management_System.Models.Entities
{
    public class Submission
    {
        public int SubId { get; set; }
        public string FilePath { get; set; }
        public double? grade { get; set; }
        public string StuId { get; set; }
        public int AssignmentId { get; set; }
        public Assignment? assignment { get; set; }
        public Student? student { get; set; }
    }
}
