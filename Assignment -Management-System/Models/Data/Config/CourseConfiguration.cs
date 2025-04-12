using Assignment__Management_System.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Assignment__Management_System.Models.Data.Config
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(c => c.CrsId);

            builder.HasOne(c => c.instructor)
                .WithMany(i => i.Courses)
                .HasForeignKey(c => c.InstId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
