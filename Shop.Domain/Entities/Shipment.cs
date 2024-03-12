namespace Shop.Domain.Entities;

using Events;
using Primitives;

public record ShipmentId(Guid Value);

public class Shipment : BaseEntity
{
    public ShipmentId Id { get; init; }
    public OrderId OrderId { get; init; }
    public DateTime? DateDispatched { get; private set; }
    public DateTime? DateDelivered { get; private set; }
    
    public Shipment(ShipmentId id, OrderId orderId)
    {
        Id = id;
        OrderId = orderId;
    }

    public void SetDispatchDate(DateTime dispatchDate)
    {
        if (DateDispatched != null) return;
        DateDispatched = dispatchDate;
        AddDomainEvent(new ShipmentDispatched(this));
    }
    
    public void SetDeliveryDate(DateTime deliveryDate)
    {
        if (DateDelivered != null) return;
        DateDelivered = deliveryDate;
        AddDomainEvent(new ShipmentDelivered(this));
    }
    
    private Shipment() {}
}