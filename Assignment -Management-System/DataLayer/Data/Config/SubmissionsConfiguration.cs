using Assignment__Management_System.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Assignment__Management_System.Models.Data.Config
{
    public class SubmissionsConfiguration : IEntityTypeConfiguration<Submission>
    {
        public void Configure(EntityTypeBuilder<Submission> builder)
        {
            builder.HasKey(s => s.SubId);

            builder.Property(s => s.grade)
                .IsRequired(false);

            builder.HasOne(s => s.assignment)
                .WithMany(a => a.Submissions)
                .HasForeignKey(s => s.AssignmentId)
                .OnDelete(DeleteBehavior.Cascade); 

            builder.HasOne(s => s.student)
                .WithMany(s => s.Submissions)
                .HasForeignKey(s => s.StuId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
