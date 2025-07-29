using Assignment__Management_System.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Assignment__Management_System.Models.Data.Config
{
    public class NotificationsConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasKey(n => n.NotifId);

            builder.Property(n => n.IsRead)
                .IsRequired(false);

            builder.HasOne(n => n.Reciver)
                .WithMany(r => r.Notifications)
                .HasForeignKey(r => r.ReciverId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
