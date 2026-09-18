using Microsoft.AspNetCore.Http;

namespace Assignment__Management_System.DataLayer.DTOs
{
    public class ImageUploadDTO
    {
        public IFormFile Image { get; set; }
    }
}
