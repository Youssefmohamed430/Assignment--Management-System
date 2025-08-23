using Assignment__Management_System.DataLayer;
using Assignment__Management_System.DataLayer.DTOs;
using Assignment__Management_System.Factories;
using Assignment__Management_System.Models.Data;
using Assignment__Management_System.Models.Entities;
using Azure;
using Microsoft.EntityFrameworkCore;

namespace Assignment__Management_System.Services
{
    public class NotificationService : INotificationService
    {
        private readonly AppDbContext context;

        public NotificationService(AppDbContext context)
        {
            this.context = context;
        }
        public void CreateNotification(string userId, string message)
        {
            var notif = new Notifications
            {
                ReciverId = userId,
                Message = message
            };

            context.Notifications.Add(notif);
            context.SaveChanges();
        }

        public void NotifyStudentsOfNewAssignment(Assignment assignment)
        {
            var students = context.CourseEnrollments
                .Include(e => e.course)
                .Where(e => e.CrsId == assignment.CrsId);

            foreach (var stud in students)
            {
                CreateNotification(stud.StuId,
                    $"There is a new assignment {assignment.Title} for the course {stud.course.CrsName} and an dead line is  {assignment.DeadLine:dd/MM/yyyy}.");
            }
        }

        public ResponseModel<IQueryable<NotificationDTO>> GetNotifications(string stuid)
        {
            var notifs = context.Notifications
                .AsNoTracking()
                .Where(n => n.ReciverId == stuid && !n.IsRead)
                .Select(n => new NotificationDTO
                {
                    Id = n.NotifId,
                    Message = n.Message,
                    IsRead = n.IsRead,
                    SendDate = n.SendDate.ToString("dd/MM/yyyy HH:mm"),
                    ReciverId = n.ReciverId
                });

            if(notifs.Any())    
                return new ResponseModelFactory()
                    .CreateResponseModel<IQueryable<NotificationDTO>>(true,"", notifs);
            else
                return new ResponseModelFactory()
                    .CreateResponseModel<IQueryable<NotificationDTO>>(false, "There are no notifications for this user.", null);
        }

        public ResponseModel<NotificationDTO> MarkasRead(string userId, int notifid)
        {
            var notif = context.Notifications
                .FirstOrDefault(n => n.ReciverId == userId && n.NotifId == notifid);

            if (notif == null)
            {
                return new ResponseModelFactory()
                    .CreateResponseModel<NotificationDTO>(false, "Notification not found", null);
            }
            notif.IsRead = true;
            context.Notifications.Update(notif);
            context.SaveChanges();

            var notificationDto = new NotificationDTO
            {
                Message = notif.Message,
                IsRead = notif.IsRead,
                SendDate = notif.SendDate.ToString("dd/MM/yyyy HH:mm"),
                ReciverId = notif.ReciverId
            };

            return new ResponseModelFactory()
                .CreateResponseModel<NotificationDTO>(true, "Notification marked as read", notificationDto);
        }
    }
}
