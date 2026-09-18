using Assignment__Management_System.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Assignment__Management_System.Models.Data.Config
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(a => a.Name)
                   .HasMaxLength(255)
                   .IsRequired();

            builder.Property(a => a.Email)
                   .HasMaxLength(255)
                   .IsRequired();

            builder.Property(a => a.UserName)
                   .HasColumnType("nvarchar(256)")
                   .HasMaxLength(255)
                   .IsRequired();
        }
    }
}
