using Assignment__Management_System.DataLayer.DTOs;
using Assignment__Management_System.DataLayer;
using Assignment__Management_System.Models.Entities;

namespace Assignment__Management_System.Services
{
    public interface INotificationService
    {
        void NotifyStudentsOfNewAssignment(Assignment assignment);
        void CreateNotification(string userId, string message);
        ResponseModel<IQueryable<NotificationDTO>> GetNotifications(string stuid);
        ResponseModel<NotificationDTO> MarkasRead(string userId, int notifid);
    }
}
