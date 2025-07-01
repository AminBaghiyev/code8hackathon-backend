using Management.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Management.DL.Configurations;

public class ServiceConfiguration : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder
            .Property(p => p.Name)
            .HasMaxLength(100);

        builder
            .Property(p => p.Price)
            .HasColumnType("decimal(10,2)");
    }
}
