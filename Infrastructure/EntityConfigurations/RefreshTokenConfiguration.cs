using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.UserId)
            .IsRequired();

        builder.Property(i => i.Token)
            .IsRequired()
            .HasMaxLength(512);

        builder.Property(i => i.ReplacedByToken)
            .HasMaxLength(512);

        builder.Property(i => i.Expires)
            .IsRequired();

        builder.Property(i => i.JwtId)
            .HasMaxLength(512);

        builder.Property(i => i.DeviceName)
            .HasMaxLength(150);

        builder.Property(i => i.Platform)
            .HasMaxLength(150);

        builder.Property(i => i.IpAddress)
            .HasMaxLength(150);

        builder.Property(i => i.Browser)
            .HasMaxLength(150);
    }
}
