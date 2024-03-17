namespace Shop.Application.UnitTests.Shipments.Commands;

using Domain.Entities;
using Domain.Enums;
using Application.Shipments.Commands;

public class CreateShipmentTests
{
    private readonly TestDbContext _context;
    private readonly CreateShipmentHandler _handler;
    private readonly CreateShipmentValidator _validator;
    
    public CreateShipmentTests()
    {
        _context = new TestDbContext();
        _handler = new CreateShipmentHandler(_context);
        _validator = new CreateShipmentValidator();
    }

    [Fact]
    public async Task CreateShipment_ShouldSucceed_WhenRequestValid()
    {
        var orderId = Guid.NewGuid();
        
        var order = new Order
        (
            id: new OrderId(orderId),
            checkoutId: new CheckoutId("checkoutId"),
            userId: new UserId(Guid.NewGuid()),
            status: OrderStatus.Processing,
            amount: 50m
        );
        
        _context.Orders.Add(order);
        
        await _context.SaveChangesAsync();

        var command = new CreateShipmentCommand(orderId);

        var result = await _handler.Handle(command, default);

        result.IsSuccess.Should().BeTrue();
        result.Value.OrderId.Value.Should().Be(orderId);
    }

    [Fact]
    public async Task CreateShipment_ShouldFail_WhenOrderNotFound()
    {
        var command = new CreateShipmentCommand(Guid.NewGuid());

        var result = await _handler.Handle(command, default);

        result.IsFailed.Should().BeTrue();
    }
    
    [Fact]
    public void CreateShipment_ShouldReturnError_WhenOrderIdEmpty()
    {
        var command = new CreateShipmentCommand(Guid.Empty);
        
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.OrderId);
    }
}