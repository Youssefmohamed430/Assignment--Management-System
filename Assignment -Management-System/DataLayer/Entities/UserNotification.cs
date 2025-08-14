using Assignment__Management_System.Models.Entities;

namespace Assignment__Management_System.DataLayer.Entities
{
    public class UserNotification
    {
        public int NotifId { get; set; }
        public string UserId { get; set; }
        public bool IsRead { get; set; } = false;
        public Notifications notification { get; set; }
        public ApplicationUser user { get; set; }
    }
}
