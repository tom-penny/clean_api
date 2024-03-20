using Bogus;

namespace Shop.Infrastructure.Data;

using Domain.Enums;
using Domain.Entities;
using Application.Common.Interfaces;

public class ApplicationDbSeeder
{
    private readonly IApplicationDbContext _context;

    public ApplicationDbSeeder(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        await SeedCategoriesAsync();
        await SeedUsersAsync();
        await SeedProductsAsync();
        await SeedOrdersAsync();
        await SeedPaymentsAsync();
        await SeedShipmentsAsync();
    }

    private async Task SeedCategoriesAsync()
    {
        if (_context.Categories.Any()) return;

        var categories = new Faker<Category>()
            .CustomInstantiator(f => new Category
            (
                id: new CategoryId(Guid.NewGuid()),
                name: f.Commerce.Department()
            ))
            .Generate(10);

        await _context.Categories.AddRangeAsync(categories);
        
        await _context.SaveChangesAsync(default);
    }
    
    private async Task SeedUsersAsync()
    {
        if (_context.Users.Any()) return;

        var users = new Faker<User>()
            .CustomInstantiator(f => new User
            (
                id: new UserId(Guid.NewGuid()),
                firstName: f.Name.FirstName(),
                lastName: f.Name.LastName(),
                email: f.Internet.Email()
            ))
            .Generate(10);

        await _context.Users.AddRangeAsync(users);
        
        await _context.SaveChangesAsync(default);
    }
    
    private async Task SeedProductsAsync()
    {
        if (_context.Products.Any()) return;

        var categories = await _context.Categories.ToListAsync();
        
        var products = new Faker<Product>()
            .CustomInstantiator(f => new Product
            (
                id: new ProductId(Guid.NewGuid()),
                name: f.Commerce.ProductName(),
                stock: f.Random.Number(1, 100),
                price: f.Finance.Amount(5, 500),
                categories: f.Random.ListItems(categories,
                    f.Random.Int(1, categories.Count)).ToList()
            ))
            .Generate(50);

        await _context.Products.AddRangeAsync(products);
        
        await _context.SaveChangesAsync(default);
    }

    private async Task SeedOrdersAsync()
    {
        if (_context.Orders.Any()) return;

        var users = await _context.Users.ToListAsync();

        var products = await _context.Products.ToListAsync();

        var orders = new Faker<Order>()
            .CustomInstantiator(f =>
            {
                var user = f.PickRandom(users);

                var status = f.PickRandom<OrderStatus>();

                var selections = f.Random.ListItems(products, f.Random.Int(1, 5)).ToHashSet();

                var quantities = selections.Select(p => (p, f.Random.Int(1, 10))).ToList();

                var total = quantities.Sum(pq => pq.Item1.Price * pq.Item2);

                var order = new Order
                (
                    id: new OrderId(Guid.NewGuid()),
                    checkoutId: new CheckoutId(f.Random.AlphaNumeric(20)),
                    userId: user.Id,
                    amount: total,
                    status: status
                );

                foreach (var (product, quantity) in quantities)
                {
                    var orderItem = new OrderItem
                    (
                        id: new OrderItemId(Guid.NewGuid()),
                        orderId: order.Id,
                        productId: product.Id,
                        unitPrice: product.Price,
                        quantity: quantity
                    );

                    _context.OrderItems.Add(orderItem);
                }

                return order;
            })
            .Generate(20);

        await _context.Orders.AddRangeAsync(orders);

        await _context.SaveChangesAsync(default);
    }

    private async Task SeedPaymentsAsync()
    {
        if (_context.Payments.Any()) return;
        
        var validStatuses = new[]
        {
            OrderStatus.Confirmed,
            OrderStatus.Completed,
            OrderStatus.Processing
        };
        
        var orders = _context.Orders.Where(o =>
            validStatuses.Contains(o.Status)).ToList();
        
        foreach (var order in orders)
        {
            var payment = new Payment
            (
                id: new PaymentId(Guid.NewGuid()),
                checkoutId: order.CheckoutId,
                amount: order.Amount,
                status: PaymentStatus.Approved
            );

            _context.Payments.Add(payment);
        }

        await _context.SaveChangesAsync(default);
    }

    private async Task SeedShipmentsAsync()
    {
        if (_context.Shipments.Any()) return;

        var validStatuses = new[]
        {
            OrderStatus.Completed,
            OrderStatus.Processing
        };

        var today = DateTime.UtcNow;

        var faker = new Faker();

        var orders = _context.Orders.Where(o =>
            validStatuses.Contains(o.Status)).ToList();

        foreach (var order in orders)
        {
            var dispatched = faker.Date.Between(today.AddDays(-20), today);

            var shipment = new Shipment
            (
                id: new ShipmentId(Guid.NewGuid()),
                orderId: order.Id
            );

            shipment.SetDispatchDate(dispatched);

            if (order.Status == OrderStatus.Completed)
            {
                shipment.SetDeliveryDate(faker.Date.Between(today.AddDays(-20), today));
            }

            _context.Shipments.Add(shipment);
        }

        await _context.SaveChangesAsync(default);
    }
}