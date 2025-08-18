using Microsoft.AspNetCore.Identity;

namespace Assignment__Management_System.Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public Student student { get; set; }
        public Instructor instructor { get; set; }
    }
}
