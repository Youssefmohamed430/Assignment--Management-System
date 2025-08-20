using System.ComponentModel.DataAnnotations;

namespace Assignment__Management_System.DataLayer.DTOs
{
    public class CourseDto
    {
        public int? Id { get; set; }
        [Required]
        public string CrsName { get; set; }
        [Required]
        public string InstId { get; set; }
        public string? InstName { get; set; }
    }
}
