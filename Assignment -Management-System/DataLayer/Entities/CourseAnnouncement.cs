namespace Assignment__Management_System.Models.Entities
{
    public class CourseAnnouncement
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Course? Course { get; set; }
    }
}
