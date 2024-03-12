using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Shop.Infrastructure.Data;

using Outbox;
using Application.Interfaces;
using Domain.Entities;
using Domain.Primitives;

public class ApplicationDbContext : IdentityDbContext<IdentityUser<Guid>, IdentityRole<Guid>, Guid>, IApplicationDbContext
{
    private readonly IMediator _mediator;
    
    public DbSet<Category> Categories { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Shipment> Shipments { get; set; }
    public DbSet<User> Users { get; set; }
    
    public DbSet<OutboxMessage> OutboxMessages { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IMediator mediator) : base(options)
    {
        _mediator = mediator;
    }

    // public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new ())
    // {
    //     var domainEntities = ChangeTracker.Entries<BaseEntity>()
    //         .Where(e => e.Entity.Events.Any()).ToList();
    //     
    //     // Retrieve domain events for updated entities.
    //
    //     var domainEvents = domainEntities.SelectMany(e => e.Entity.Events).ToList();
    //     
    //     // Clear domain events for updated entities.
    //
    //     domainEntities.ForEach(e => e.Entity.ClearDomainEvents());
    //     
    //     var result = await base.SaveChangesAsync(cancellationToken);
    //     
    //     // Publish domain events after changes persisted.
    //     
    //     foreach (var domainEvent in domainEvents)
    //     {
    //         await _mediator.Publish(domainEvent, cancellationToken);
    //     }
    //     
    //     // domainEntities.ForEach(e => e.Entity.ClearDomainEvents());
    //     
    //     return result;
    // }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.Property(e => e.Id).HasConversion(v => v.Value, v => new CategoryId(v));
        });
        
        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(e => e.Id).HasConversion(v => v.Value, v => new OrderId(v));
            entity.Property(e => e.CheckoutId).HasConversion(v => v.Value, v => new CheckoutId(v));
            entity.Property(e => e.UserId).HasConversion(v => v.Value, v => new UserId(v));
            entity.Property(e => e.PaymentId).HasConversion(v => v.Value, v => new PaymentId(v));
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.Property(e => e.Id).HasConversion(v => v.Value, v => new OrderItemId(v));
            entity.Property(e => e.OrderId).HasConversion(v => v.Value, v => new OrderId(v));
            entity.Property(e => e.ProductId).HasConversion(v => v.Value, v => new ProductId(v));
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.Property(e => e.Id).HasConversion(v => v.Value, v => new PaymentId(v));
            entity.Property(e => e.CheckoutId).HasConversion(v => v.Value, v => new CheckoutId(v));
        });
        
        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.Id).HasConversion(v => v.Value, v => new ProductId(v));
        });

        modelBuilder.Entity<Shipment>(entity =>
        {
            entity.Property(e => e.Id).HasConversion(v => v.Value, v => new ShipmentId(v));
            entity.Property(e => e.OrderId).HasConversion(v => v.Value, v => new OrderId(v));
        });
        
        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Id).HasConversion(v => v.Value, v => new UserId(v));
        });
        
        base.OnModelCreating(modelBuilder);
        
        // modelBuilder.AddInboxStateEntity();
        // modelBuilder.AddOutboxStateEntity();
        // modelBuilder.AddOutboxMessageEntity();
    }
}