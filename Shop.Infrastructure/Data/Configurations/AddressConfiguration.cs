using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Shop.Infrastructure.Data.Configurations;

using Domain.Entities;

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.Property(e => e.Id).HasConversion(v => v.Value, v => new AddressId(v));
        builder.Property(e => e.UserId).HasConversion(v => v.Value, v => new UserId(v));
        builder.HasIndex(e => e.UserId).HasDatabaseName("IDX_Address_UserId");
    }
}