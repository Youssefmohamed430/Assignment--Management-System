namespace Assignment__Management_System.Models.Entities
{
    public class Student
    {
        public string Id { get; set; }
        public ApplicationUser User { get; set; }
        public List<CourseEnrollments> enrollments { get; set; }
        public List<Submission> Submissions { get; set; }
    }
}
