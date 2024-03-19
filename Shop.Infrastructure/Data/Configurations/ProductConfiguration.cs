using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Shop.Infrastructure.Data.Configurations;

using Domain.Entities;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(e => e.Id).HasConversion(v => v.Value, v => new ProductId(v));
        builder.HasIndex(e => e.Name).HasDatabaseName("IDX_Product_Name");
        builder.HasIndex(e => e.Price).HasDatabaseName("IDX_Product_Price");
        builder.HasIndex(e => e.Created).HasDatabaseName("IDX_Product_Created");
    }
}