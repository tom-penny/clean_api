using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Shop.Infrastructure.Data.Configurations;

using Domain.Entities;

public class ShipmentConfiguration : IEntityTypeConfiguration<Shipment>
{
    public void Configure(EntityTypeBuilder<Shipment> builder)
    {
        builder.Property(e => e.Id).HasConversion(v => v.Value, v => new ShipmentId(v));
        builder.Property(e => e.OrderId).HasConversion(v => v.Value, v => new OrderId(v));
    }
}