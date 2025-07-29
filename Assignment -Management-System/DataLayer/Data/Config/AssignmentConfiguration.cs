using Assignment__Management_System.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Assignment__Management_System.Models.Data.Config
{
    public class AssignmentConfiguration : IEntityTypeConfiguration<Assignment>
    {
        public void Configure(EntityTypeBuilder<Assignment> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(a => a.Title)
                   .HasMaxLength(55)
                   .IsRequired();

            builder.HasOne(a => a.course)
                .WithMany(c => c.assignments)
                .HasForeignKey(a => a.CrsId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
