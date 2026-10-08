using Assignment__Management_System.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Assignment__Management_System.Models.Data.Config
{
    public class CourseAnnouncementConfiguration : IEntityTypeConfiguration<CourseAnnouncement>
    {
        public void Configure(EntityTypeBuilder<CourseAnnouncement> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Message)
                .IsRequired()
                .HasMaxLength(2000);
            builder.Property(a => a.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
            builder.HasOne(a => a.Course)
                .WithMany(c => c.Announcements)
                .HasForeignKey(a => a.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(a => new { a.CourseId, a.CreatedAt });
        }
    }
}
