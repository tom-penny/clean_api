namespace Shop.Domain.UnitTests.Entities;

using Domain.Entities;
using Domain.Events;

public class ShipmentTests
{
    private readonly Shipment _shipment;

    public ShipmentTests()
    {
        _shipment = new Shipment
        (
            id: new ShipmentId(Guid.NewGuid()),
            orderId: new OrderId(Guid.NewGuid())
        );
    }
    
    [Fact]
    public void SetDispatchDate_ShouldSetDateDispatched()
    {
        var dispatchDate = DateTime.Now;
        
        _shipment.SetDispatchDate(dispatchDate);

        _shipment.DateDispatched.Should().Be(dispatchDate);
    }
    
    [Fact]
    public void SetDispatchDate_ShouldAddShipmentDispatchedEvent()
    {
        _shipment.SetDispatchDate(DateTime.Now);

        _shipment.Events.Should().Contain(e =>
            e.GetType() == typeof(ShipmentDispatched));
    }

    [Fact]
    public void SetDispatchDate_ShouldBeIdempotent()
    {
        var dispatchDate = DateTime.Now;

        _shipment.SetDispatchDate(dispatchDate);

        _shipment.SetDispatchDate(dispatchDate.AddDays(1));

        _shipment.DateDispatched.Should().Be(dispatchDate);
        _shipment.Events.OfType<ShipmentDispatched>().Count().Should().Be(1);
    }
    
    [Fact]
    public void SetDeliveryDate_ShouldSetDateDelivered()
    {
        var deliveryDate = DateTime.Now;
        
        _shipment.SetDeliveryDate(deliveryDate);

        _shipment.DateDelivered.Should().Be(deliveryDate);
    }

    [Fact]
    public void SetDeliveryDate_ShouldAddShipmentDeliveredEvent()
    {
        _shipment.SetDeliveryDate(DateTime.Now);

        _shipment.Events.Should().Contain(e =>
            e.GetType() == typeof(ShipmentDelivered));
    }
    
    [Fact]
    public void SetDeliveryDate_ShouldBeIdempotent()
    {
        var deliveryDate = DateTime.Now;

        _shipment.SetDeliveryDate(deliveryDate);

        _shipment.SetDeliveryDate(deliveryDate.AddDays(1));

        _shipment.DateDelivered.Should().Be(deliveryDate);
        _shipment.Events.OfType<ShipmentDelivered>().Count().Should().Be(1);
    }
}