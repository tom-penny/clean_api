namespace Shop.Application.UnitTests.Orders.Queries;

using Domain.Entities;
using Domain.Enums;
using Application.Orders.Queries;

public class GetAllOrdersTests
{
    private readonly TestDbContext _context;
    private readonly GetAllOrdersHandler _handler;
    private readonly GetAllOrdersValidator _validator;

    public GetAllOrdersTests()
    {
        _context = new TestDbContext();
        _handler = new GetAllOrdersHandler(_context);
        _validator = new GetAllOrdersValidator();
    }

    [Fact]
    public async Task GetAllOrders_ShouldSucceed_WhenRequestValid()
    {
        var userId = Guid.NewGuid();
        
        var orders = new List<Order>
        {
            new
            (
                id: new OrderId(Guid.NewGuid()),
                checkoutId: new CheckoutId("checkoutId1"),
                userId: new UserId(userId),
                amount: 50m,
                status: OrderStatus.Pending
            ),
            new
            (
                id: new OrderId(Guid.NewGuid()),
                checkoutId: new CheckoutId("checkoutId2"),
                userId: new UserId(userId),
                amount: 80m,
                status: OrderStatus.Pending
            ),
        };
        
        _context.Orders.AddRange(orders);
        
        await _context.SaveChangesAsync();
        
        var query = new GetAllOrdersQuery
        {
            UserId = userId,
            SortBy = "total",
            OrderBy = "asc",
            Page = 1,
            Size = 10
        };

        var result = await _handler.Handle(query, default);

        result.IsSuccess.Should().BeTrue();
        result.Value.Count.Should().Be(2);
    }

    [Fact]
    public async Task GetAllOrders_ShouldSucceed_WhenOrdersEmpty()
    {
        var query = new GetAllOrdersQuery
        {
            UserId = Guid.NewGuid(),
            SortBy = "total",
            OrderBy = "asc",
            Page = 1,
            Size = 10
        };
        var result = await _handler.Handle(query, default);

        result.IsSuccess.Should().BeTrue();
        result.Value.Count.Should().Be(0);
    }
    
    [Fact]
    public void GetAllOrders_ShouldReturnError_WhenUserIdEmpty()
    {
        var query = new GetAllOrdersQuery
        {
            UserId = Guid.Empty,
            SortBy = "total",
            OrderBy = "asc",
            Page = 1,
            Size = 10
        };        
        var result = _validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(q => q.UserId);
    }
    
    [Fact]
    public void GetAllOrders_ShouldReturnError_WhenSortByInvalid()
    {
        var query = new GetAllOrdersQuery
        {
            UserId = Guid.NewGuid(),
            SortBy = "invalid",
            OrderBy = "asc",
            Page = 1,
            Size = 10
        };

        var result = _validator.TestValidate(query);
        
        result.ShouldHaveValidationErrorFor(q => q.SortBy);
    }
    
    [Fact]
    public void GetAllOrders_ShouldReturnError_WhenOrderByInvalid()
    {
        var query = new GetAllOrdersQuery
        {
            UserId = Guid.NewGuid(),
            SortBy = "total",
            OrderBy = "invalid",
            Page = 1,
            Size = 10
        };

        var result = _validator.TestValidate(query);
        
        result.ShouldHaveValidationErrorFor(q => q.OrderBy);
    }
    
    [Fact]
    public void GetAllOrders_ShouldReturnError_WhenPageZero()
    {
        var query = new GetAllOrdersQuery
        {
            UserId = Guid.NewGuid(),
            SortBy = "total",
            OrderBy = "asc",
            Page = 0,
            Size = 10
        };

        var result = _validator.TestValidate(query);
        
        result.ShouldHaveValidationErrorFor(q => q.Page);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(26)]
    public void GetAllOrders_ShouldReturnError_WhenSizeInvalid(int size)
    {
        var query = new GetAllOrdersQuery
        {
            UserId = Guid.NewGuid(),
            SortBy = "total",
            OrderBy = "asc",
            Page = 1,
            Size = size
        };

        var result = _validator.TestValidate(query);
        
        result.ShouldHaveValidationErrorFor(q => q.Size);
    }
}