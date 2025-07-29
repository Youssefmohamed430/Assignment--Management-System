using Microsoft.AspNetCore.Antiforgery;
using System.ComponentModel.DataAnnotations;

namespace Assignment__Management_System.DataLayer.DTOs
{
    public class UserDto
    {
        [Required]
        [Length(3,25,ErrorMessage ="User Name Must be between 3 to 25 character!")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Username must contain only letters and numbers")]
        public string UserName { get; set; }
        [Required]
        [Length(3,25,ErrorMessage ="Name Must be between 3 to 25 character!")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Name must contain only letters")]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "password must contain only letters and numbers")]
        public string Password { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "In Valid Email")]
        public string Email { get; set; }
        [Required]
        public string Role { get; set; }
    }
}