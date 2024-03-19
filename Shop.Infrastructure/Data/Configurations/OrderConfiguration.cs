using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Shop.Infrastructure.Data.Configurations;

using Domain.Entities;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(e => e.Id).HasConversion(v => v.Value, v => new OrderId(v));
        builder.Property(e => e.CheckoutId).HasConversion(v => v.Value, v => new CheckoutId(v));
        builder.Property(e => e.UserId).HasConversion(v => v.Value, v => new UserId(v));
        builder.Property(e => e.PaymentId).HasConversion(v => v.Value, v => new PaymentId(v));
    }
}