using Assignment__Management_System.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Assignment__Management_System.Models.Data.Config
{
    public class CourseEnrollmentsConfiguration : IEntityTypeConfiguration<CourseEnrollments>
    {
        public void Configure(EntityTypeBuilder<CourseEnrollments> builder)
        {
            builder.HasKey(e => new { e.StuId, e.CrsId });

            builder.HasOne(e => e.student)
               .WithMany(s => s.enrollments)
               .HasForeignKey(e => e.StuId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.course)
                .WithMany(c => c.courseEnrollments)
                .HasForeignKey(e => e.CrsId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
