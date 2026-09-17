using Microsoft.AspNetCore.Http;

namespace Assignment__Management_System.DataLayer.DTOs
{
    public class InstructorDTO
    {
        public string? id { get; set; }
        public string Name { get; set; }
        public string? ImageName { get; set; }
        public IFormFile? Image { get; set; }
    }
}
