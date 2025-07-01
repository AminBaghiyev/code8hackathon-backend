using Management.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Management.DL.Configurations;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder
            .Property(e => e.FullName)
            .HasMaxLength(100);

        builder
            .Property(e => e.PhoneNumber)
            .HasMaxLength(20);
    }
}
