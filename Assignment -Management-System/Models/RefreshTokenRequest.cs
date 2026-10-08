using System.ComponentModel.DataAnnotations;

namespace Assignment__Management_System.Models
{
    public class RefreshTokenRequest
    {
        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
