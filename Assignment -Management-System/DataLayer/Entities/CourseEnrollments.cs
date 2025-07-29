namespace Assignment__Management_System.Models.Entities
{
    public class CourseEnrollments
    {
        public int CrsId { get; set; }
        public string StuId { get; set; }
        public Student? student { get; set; }
        public Course? course { get; set; }
    }
}
