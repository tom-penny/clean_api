namespace Shop.Application.UnitTests.Orders.Commands;

using Domain.Entities;
using Application.Orders.Commands;

public class CreateOrderTests
{
    private readonly TestDbContext _context;
    private readonly CreateOrderHandler _handler;
    private readonly CreateOrderValidator _validator;

    public CreateOrderTests()
    {
        _context = new TestDbContext();
        _handler = new CreateOrderHandler(_context);
        _validator = new CreateOrderValidator();
    }
    
    [Fact]
    public async Task CreateOrder_ShouldSucceed_WhenRequestValid()
    {
        var userId = Guid.NewGuid();

        var user = new User
        (
            id: new UserId(userId),
            firstName: "firstName",
            lastName: "lastName",
            email: "test@test.com"
        );
        
        _context.Users.Add(user);
        
        await _context.SaveChangesAsync();
        
        var command = new CreateOrderCommand
        {
            CheckoutId = "checkoutId",
            UserId = userId,
            Amount = 50m,
            Items = new List<CreateOrderItem>()
        };

        var result = await _handler.Handle(command, default);

        result.IsSuccess.Should().BeTrue();
        result.Value.UserId.Value.Should().Be(userId);
    }
    
    [Fact]
    public void CreateOrder_ShouldReturnError_WhenCheckoutIdEmpty()
    {
        var command = new CreateOrderCommand
        {
            CheckoutId = string.Empty,
            UserId = Guid.NewGuid(),
            Amount = 50m,
            Items = new List<CreateOrderItem>()
        };
        
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.CheckoutId);
    }
    
    [Fact]
    public void CreateOrder_ShouldReturnError_WhenUserIdEmpty()
    {
        var command = new CreateOrderCommand
        {
            CheckoutId = "checkoutId",
            UserId = Guid.Empty,
            Amount = 50m,
            Items = new List<CreateOrderItem>()
        };
        
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.UserId);
    }
    
    [Fact]
    public void CreateOrder_ShouldReturnError_WhenAmountZero()
    {
        var command = new CreateOrderCommand
        {
            CheckoutId = "checkoutId",
            UserId = Guid.NewGuid(),
            Amount = decimal.Zero,
            Items = new List<CreateOrderItem>()
        };
        
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Amount);
    }
    
    [Fact]
    public void CreateOrder_ShouldReturnError_WhenItemsEmpty()
    {
        var command = new CreateOrderCommand
        {
            CheckoutId = "checkoutId",
            UserId = Guid.NewGuid(),
            Amount = 50m,
            Items = new List<CreateOrderItem>()
        };
        
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Items);
    }
    
    [Fact]
    public void CreateOrder_ShouldReturnError_WhenAnyItemProductIdEmpty()
    {
        var command = new CreateOrderCommand
        {
            CheckoutId = "checkoutId",
            UserId = Guid.NewGuid(),
            Amount = 50m,
            Items = new List<CreateOrderItem> { new (Guid.Empty, 5, 10m) }
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor("Items[0].ProductId");
    }
    
    [Fact]
    public void CreateOrder_ShouldReturnError_WhenAnyItemQuantityZero()
    {
        var command = new CreateOrderCommand
        {
            CheckoutId = "checkoutId",
            UserId = Guid.NewGuid(),
            Amount = 50m,
            Items = new List<CreateOrderItem> { new (Guid.NewGuid(), 0, 10m) }
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor("Items[0].Quantity");
    }
    
    [Fact]
    public void CreateOrder_ShouldReturnError_WhenAnyItemUnitPriceZero()
    {
        var command = new CreateOrderCommand
        {
            CheckoutId = "checkoutId",
            UserId = Guid.NewGuid(),
            Amount = 50m,
            Items = new List<CreateOrderItem> { new (Guid.NewGuid(), 5, 0.0m) }
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor("Items[0].UnitPrice");
    }
}