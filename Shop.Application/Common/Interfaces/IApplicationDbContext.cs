namespace Shop.Application.Common.Interfaces;

using Domain.Entities;

// Abstraction of EF database context, implemented in Shop.Infrastructure.

public interface IApplicationDbContext
{
    DbSet<Category> Categories { get; }
    DbSet<Order> Orders { get; }
    DbSet<OrderItem> OrderItems { get; }
    DbSet<Payment> Payments { get; }
    DbSet<Product> Products { get; }
    DbSet<Shipment> Shipments { get; }
    DbSet<User> Users { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}   