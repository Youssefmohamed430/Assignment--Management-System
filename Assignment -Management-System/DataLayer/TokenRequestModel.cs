using System.ComponentModel.DataAnnotations;

namespace Assignment__Management_System.Models
{
    public class TokenRequestModel
    {
        [Required]
        public string Username { get; set; }
        [Required] 
        public string password { get; set; }
    }
}