using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Shop.Infrastructure.Data;

using Outbox;
using Domain.Entities;
using Application.Common.Interfaces;

public class ApplicationDbContext : IdentityDbContext<IdentityUser<Guid>,
    IdentityRole<Guid>, Guid>, IApplicationDbContext
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Shipment> Shipments { get; set; }
    public DbSet<User> Users { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}