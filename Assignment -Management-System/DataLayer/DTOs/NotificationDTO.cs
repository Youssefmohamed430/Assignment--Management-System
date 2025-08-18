namespace Assignment__Management_System.DataLayer.DTOs
{
    public class NotificationDTO
    {
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public string SendDate { get; set; } 
        public string ReciverId { get; set; }
    }
}
