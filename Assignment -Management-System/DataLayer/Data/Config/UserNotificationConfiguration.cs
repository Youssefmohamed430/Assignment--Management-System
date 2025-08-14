using Assignment__Management_System.DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Assignment__Management_System.DataLayer.Data.Config
{
    public class UserNotificationConfiguration : IEntityTypeConfiguration<UserNotification>
    {
        public void Configure(EntityTypeBuilder<UserNotification> builder)
        {
            builder.HasKey(n => new { n.UserId, n.NotifId });

            builder.HasOne(x => x.user)
                .WithMany(x => x.Notifications)
                .HasForeignKey(x => x.UserId);

            builder.HasOne(x => x.notification)
                .WithMany(x => x.usernotif)
                .HasForeignKey(x => x.NotifId);
        }
    }
}
