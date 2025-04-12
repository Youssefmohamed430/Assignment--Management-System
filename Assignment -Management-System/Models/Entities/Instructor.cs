namespace Assignment__Management_System.Models.Entities
{
    public class Instructor
    {
        public string Id { get; set; }
        public ApplicationUser? User { get; set; }
        public List<Course>? Courses { get; set; }
    }
}
