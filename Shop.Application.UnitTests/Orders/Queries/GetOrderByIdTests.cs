namespace Shop.Application.UnitTests.Orders.Queries;

using Domain.Entities;
using Domain.Enums;
using Application.Orders.Queries;

public class GetOrderByIdTests
{
    private readonly TestDbContext _context;
    private readonly GetOrderByIdHandler _handler;
    private readonly GetOrderByIdValidator _validator;

    public GetOrderByIdTests()
    {
        _context = new TestDbContext();
        _handler = new GetOrderByIdHandler(_context);
        _validator = new GetOrderByIdValidator();
    }

    [Fact]
    public async Task GetOrderById_ShouldSucceed_WhenRequestValid()
    {
        var orderId = Guid.NewGuid();
        
        var userId = Guid.NewGuid();

        var order1 = new Order
        (
            id: new OrderId(orderId),
            checkoutId: new CheckoutId("checkoutId1"),
            userId: new UserId(userId),
            amount: 50m,
            status: OrderStatus.Pending
        );

        var order2 = new Order
        (
            id: new OrderId(Guid.NewGuid()),
            checkoutId: new CheckoutId("checkoutId2"),
            userId: new UserId(userId),
            amount: 80m,
            status: OrderStatus.Pending
        );

        _context.Orders.AddRange(new List<Order> { order1, order2 });

        await _context.SaveChangesAsync();
        
        var query = new GetOrderByIdQuery(orderId, userId);

        var result = await _handler.Handle(query, default);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeSameAs(order1);
    }

    [Fact]
    public async Task GetOrderById_ShouldFail_WhenOrderNotFound()
    {
        var userId = Guid.NewGuid();

        var order1 = new Order
        (
            id: new OrderId(Guid.NewGuid()),
            checkoutId: new CheckoutId("checkoutId1"),
            userId: new UserId(userId),
            amount: 50m,
            status: OrderStatus.Pending
        );

        var order2 = new Order
        (
            id: new OrderId(Guid.NewGuid()),
            checkoutId: new CheckoutId("checkoutId2"),
            userId: new UserId(userId),
            amount: 80m,
            status: OrderStatus.Pending
        );
    
        _context.Orders.AddRange(new List<Order> { order1, order2 });

        await _context.SaveChangesAsync();
        
        var query = new GetOrderByIdQuery(Guid.NewGuid(), userId);
    
        var result = await _handler.Handle(query, default);
    
        result.IsFailed.Should().BeTrue();
    }
    
    [Fact]
    public void GetOrderById_ShouldReturnError_WhenOrderIdEmpty()
    {
        var query = new GetOrderByIdQuery(Guid.Empty, Guid.NewGuid());
        
        var result = _validator.TestValidate(query);
    
        result.ShouldHaveValidationErrorFor(q => q.Id);
    }
    
    [Fact]
    public void GetOrderById_ShouldReturnError_WhenUserIdEmpty()
    {
        var query = new GetOrderByIdQuery(Guid.NewGuid(), Guid.Empty);
        
        var result = _validator.TestValidate(query);
    
        result.ShouldHaveValidationErrorFor(q => q.UserId);
    }
}