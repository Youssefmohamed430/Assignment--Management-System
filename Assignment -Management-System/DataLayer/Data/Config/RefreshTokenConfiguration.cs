using Assignment__Management_System.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Assignment__Management_System.Models.Data.Config
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(token => token.Id);
            builder.Property(token => token.UserId).IsRequired().HasMaxLength(450);
            builder.Property(token => token.TokenHash).IsRequired().HasMaxLength(64);
            builder.Property(token => token.ReplacedByTokenHash).HasMaxLength(64);
            builder.Property(token => token.RowVersion).IsRowVersion();
            builder.HasIndex(token => token.TokenHash).IsUnique();
            builder.HasIndex(token => new { token.UserId, token.ExpiresAt });
            builder.HasOne(token => token.User)
                .WithMany()
                .HasForeignKey(token => token.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
