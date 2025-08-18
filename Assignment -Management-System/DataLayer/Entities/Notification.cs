
namespace Assignment__Management_System.Models.Entities
{
    public class Notifications
    {
        public int NotifId { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime SendDate { get; set; } = DateTime.Now;
        public string ReciverId { get; set; }
        public ApplicationUser? Reciver { get; set; }
    }
}
