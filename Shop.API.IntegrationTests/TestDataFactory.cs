namespace Shop.API.IntegrationTests;

using Domain.Enums;
using Domain.Entities;
using Infrastructure.Data;

public class TestDataFactory
{
    private readonly ApplicationDbContext _context;
    private readonly Faker _faker = new();

    public TestDataFactory(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Category> CreateCategoryAsync()
    {
        var category = new Category
        (
            id: new CategoryId(Guid.NewGuid()),
            name: _faker.Commerce.Department()
        );

        _context.Categories.Add(category);

        await _context.SaveChangesAsync();

        return category;
    }

    public async Task<List<Category>> CreateCategoriesAsync(int quantity = 1)
    {
        var categories = new List<Category>();
        
        for (var i = 0; i < quantity; i++)
        {
            var category = new Category
            (
                id: new CategoryId(Guid.NewGuid()),
                name: _faker.Commerce.Department()
            );
            
            categories.Add(category);
        }
        
        _context.Categories.AddRange(categories);

        await _context.SaveChangesAsync();

        return categories;
    }

    public async Task<Product> CreateProductAsync()
    {
        var category = await CreateCategoryAsync();
        
        var product = new Product
        (
            id: new ProductId(Guid.NewGuid()),
            name: _faker.Commerce.ProductName(),
            stock: _faker.Random.Int(1, 100),
            price: _faker.Finance.Amount(1m, 100m),
            categories: new List<Category> { category }
        );

        _context.Products.Add(product);

        await _context.SaveChangesAsync();

        return product;
    }

    public async Task<List<Product>> CreateProductsAsync(int quantity = 1)
    {
        var products = new List<Product>();

        var category = await CreateCategoryAsync();

        for (var i = 0; i < quantity; i++)
        {
            var product = new Product
            (
                id: new ProductId(Guid.NewGuid()),
                name: _faker.Commerce.ProductName(),
                stock: _faker.Random.Int(1, 100),
                price: _faker.Finance.Amount(1m, 100m),
                categories: new List<Category> { category }
            );
            
            products.Add(product);
        }
        
        _context.Products.AddRange(products);

        await _context.SaveChangesAsync();
        
        return products;
    }

    public async Task<User> CreateUserAsync()
    {
        var user = new User
        (
            id: new UserId(Guid.NewGuid()),
            firstName: _faker.Name.FirstName(),
            lastName: _faker.Name.LastName(),
            email: _faker.Internet.Email()
        );

        _context.Users.Add(user);

        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<List<User>> CreateUsersAsync(int quantity = 1)
    {
        var users = new List<User>();

        for (var i = 0; i < quantity; i++)
        {
            var user = new User
            (
                id: new UserId(Guid.NewGuid()),
                firstName: _faker.Name.FirstName(),
                lastName: _faker.Name.LastName(),
                email: _faker.Internet.Email()
            );
            
            users.Add(user);
        }
        
        _context.Users.AddRange(users);

        await _context.SaveChangesAsync();

        return users;
    }

    public async Task<Order> CreateOrderAsync()
    {
        var user = await CreateUserAsync();

        var product = await CreateProductAsync();

        var orderItem = new OrderItem
        (
            id: new OrderItemId(Guid.NewGuid()),
            orderId: new OrderId(Guid.NewGuid()),
            productId: product.Id,
            unitPrice: product.Price,
            quantity: _faker.Random.Int(1, 10)
        );
        
        var order = new Order
        (
            id: orderItem.OrderId,
            userId: user.Id,
            checkoutId: new CheckoutId(Guid.NewGuid().ToString()),
            amount: orderItem.UnitPrice * orderItem.Quantity,
            status: _faker.PickRandom<OrderStatus>()
        );
        
        order.Items.Add(orderItem);
        
        _context.Orders.Add(order);

        await _context.SaveChangesAsync();

        return order;
    }

    public async Task<List<Order>> CreateOrdersAsync(int quantity = 1)
    {
        var user = await CreateUserAsync();
        
        var orders = new List<Order>();

        for (var i = 0; i < quantity; i++)
        {
            var product = await CreateProductAsync();

            var orderItem = new OrderItem
            (
                id: new OrderItemId(Guid.NewGuid()),
                orderId: new OrderId(Guid.NewGuid()),
                productId: product.Id,
                unitPrice: product.Price,
                quantity: _faker.Random.Int(1, 10)
            );
        
            var order = new Order
            (
                id: orderItem.OrderId,
                userId: user.Id,
                checkoutId: new CheckoutId(Guid.NewGuid().ToString()),
                amount: orderItem.UnitPrice * orderItem.Quantity,
                status: _faker.PickRandom<OrderStatus>()
            );
        
            order.Items.Add(orderItem);
        
            orders.Add(order);
        }
        
        _context.Orders.AddRange(orders);

        await _context.SaveChangesAsync();

        return orders;
    }

    public async Task<Payment> CreatePaymentAsync()
    {
        var payment = new Payment
        (
            id: new PaymentId(Guid.NewGuid()),
            checkoutId: new CheckoutId(Guid.NewGuid().ToString()),
            amount: _faker.Finance.Amount(10m, 500m),
            status: _faker.PickRandom<PaymentStatus>()
        );

        _context.Payments.Add(payment);

        await _context.SaveChangesAsync();

        return payment;
    }

    public async Task<Shipment> CreateShipmentAsync()
    {
        var order = await CreateOrderAsync();

        var shipment = new Shipment
        (
            id: new ShipmentId(Guid.NewGuid()),
            orderId: order.Id
        );

        _context.Shipments.Add(shipment);

        await _context.SaveChangesAsync();

        return shipment;
    }

    public async Task<List<Shipment>> CreateShipmentsAsync(int quantity = 1)
    {
        var shipments = new List<Shipment>();

        for (var i = 0; i < quantity; i++)
        {
            var order = await CreateOrderAsync();

            var shipment = new Shipment
            (
                id: new ShipmentId(Guid.NewGuid()),
                orderId: order.Id
            );
            
            shipments.Add(shipment);
        }
        
        _context.Shipments.AddRange(shipments);

        await _context.SaveChangesAsync();

        return shipments;
    }
}