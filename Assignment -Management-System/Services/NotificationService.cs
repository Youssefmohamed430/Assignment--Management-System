using Assignment__Management_System.Models.Data;
using Assignment__Management_System.Models.Entities;

namespace Assignment__Management_System.Services
{
    public class NotificationService : INotificationService
    {
        private readonly AppDbContext context;

        public NotificationService(AppDbContext context)
        {
            this.context = context;
        }

        public IQueryable<Notifications> GetNotifications(string id)
        {
           //var notifs = context.UserNotifications
           //                    .Include(x => x.notification)
           //                    .Where(x => x.UserId == Id && x.IsRead == false);

           // return notifs;
           throw new NotImplementedException();
        }

        public void MarkasRead()
        {
            throw new NotImplementedException();
        }

        public void SendNotification()
        {
            throw new NotImplementedException();
        }
    }
}
