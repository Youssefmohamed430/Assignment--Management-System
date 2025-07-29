using System.ComponentModel.DataAnnotations;

namespace Assignment__Management_System.DataLayer.DTOs
{
    public class CourseDto
    {
        [Required]
        public string CrsName { get; set; }
        [Required]
        public string InstId { get; set; }
    }
}
