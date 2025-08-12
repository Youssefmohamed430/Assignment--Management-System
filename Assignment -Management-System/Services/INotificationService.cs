using Assignment__Management_System.Models.Entities;

namespace Assignment__Management_System.Services
{
    public interface INotificationService
    {
        void SendNotification();
        IQueryable<Notification> GetNotifications();
        void MarkasRead();
    }
}
