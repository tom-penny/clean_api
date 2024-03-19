using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Shop.Infrastructure.Data.Configurations;

using Domain.Entities;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.Property(e => e.Id).HasConversion(v => v.Value, v => new PaymentId(v));
        builder.Property(e => e.CheckoutId).HasConversion(v => v.Value, v => new CheckoutId(v));
    }
}