using Assignment__Management_System.DataLayer.Entities;

namespace Assignment__Management_System.Models.Entities
{
    public class Notifications
    {
        public int NotifId { get; set; }
        public string Message { get; set; }
        public DateTime SendDate { get; set; } = DateTime.Now;
        public ApplicationUser? Reciver { get; set; }
        public List<UserNotification>? usernotif { get; set; }
    }
}
