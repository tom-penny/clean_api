using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Shop.Infrastructure.Data.Configurations;

using Domain.Entities;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.Property(e => e.Id).HasConversion(v => v.Value, v => new OrderItemId(v));
        builder.Property(e => e.OrderId).HasConversion(v => v.Value, v => new OrderId(v));
        builder.Property(e => e.ProductId).HasConversion(v => v.Value, v => new ProductId(v));
    }
}