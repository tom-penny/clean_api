namespace Shop.Application.UnitTests.Shipments.Queries;

using Domain.Entities;
using Application.Shipments.Queries;

public class GetShipmentByIdTests
{
    private readonly TestDbContext _context;
    private readonly GetShipmentByIdHandler _handler;
    private readonly GetShipmentByIdValidator _validator;

    public GetShipmentByIdTests()
    {
        _context = new TestDbContext();
        _handler = new GetShipmentByIdHandler(_context);
        _validator = new GetShipmentByIdValidator();
    }

    [Fact]
    public async Task GetShipmentById_ShouldSucceed_WhenRequestValid()
    {
        var shipmentId = Guid.NewGuid();

        var shipment1 = new Shipment
        (
            id: new ShipmentId(shipmentId),
            orderId: new OrderId(Guid.NewGuid())
        );
        
        var shipment2 = new Shipment
        (
            id: new ShipmentId(Guid.NewGuid()),
            orderId: new OrderId(Guid.NewGuid())
        );
        
        _context.Shipments.AddRange(new List<Shipment> { shipment1, shipment2 });
        
        await _context.SaveChangesAsync();

        var query = new GetShipmentByIdQuery(shipmentId);

        var result = await _handler.Handle(query, default);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeSameAs(shipment1);
    }

    [Fact]
    public async Task GetShipmentById_ShouldFail_WhenShipmentNotFound()
    {
        var shipment1 = new Shipment
        (
            id: new ShipmentId(Guid.NewGuid()),
            orderId: new OrderId(Guid.NewGuid())
        );
        
        var shipment2 = new Shipment
        (
            id: new ShipmentId(Guid.NewGuid()),
            orderId: new OrderId(Guid.NewGuid())
        );
        
        _context.Shipments.AddRange(new List<Shipment> { shipment1, shipment2 });
        
        await _context.SaveChangesAsync();

        var query = new GetShipmentByIdQuery(Guid.NewGuid());

        var result = await _handler.Handle(query, default);

        result.IsFailed.Should().BeTrue();
    }

    [Fact]
    public void GetShipmentById_ShouldReturnError_WhenIdEmpty()
    {
        var query = new GetShipmentByIdQuery(Guid.Empty);
        var result = _validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(q => q.Id);
    }
}