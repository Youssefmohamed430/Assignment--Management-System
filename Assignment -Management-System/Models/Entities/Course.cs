namespace Assignment__Management_System.Models.Entities
{
    public class Course
    {   
        public int CrsId { get; set; }
        public int CrsName { get; set; }
        public string InstId { get; set; }
        public Instructor? instructor { get; set; }
        public List<Assignment>? assignments { get; set; }
        public List<CourseEnrollments>? courseEnrollments { get; set; }
    }
}
