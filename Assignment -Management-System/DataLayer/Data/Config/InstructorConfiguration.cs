using Assignment__Management_System.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Assignment__Management_System.Models.Data.Config
{
    public class InstructorConfiguration : IEntityTypeConfiguration<Instructor>
    {
        public void Configure(EntityTypeBuilder<Instructor> builder)
        {
            builder.HasKey(i => i.Id);

            builder.HasOne(u => u.User)
                .WithOne(i => i.instructor)
                .HasForeignKey<Instructor>(i => i.Id)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
