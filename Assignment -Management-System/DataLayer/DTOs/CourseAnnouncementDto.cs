using System.ComponentModel.DataAnnotations;

namespace Assignment__Management_System.DataLayer.DTOs
{
    public class CourseAnnouncementDto
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class CreateCourseAnnouncementDto
    {
        [Required]
        [StringLength(2000, MinimumLength = 1)]
        public string Message { get; set; } = string.Empty;
    }
}
