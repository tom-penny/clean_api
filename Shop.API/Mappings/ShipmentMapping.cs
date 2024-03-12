namespace Shop.API.Mappings;

using Domain.Entities;
using Models.Responses;

public static class ShipmentMapping
{
    public static ShipmentResponse ToResponse(this Shipment shipment)
    {
        return new ShipmentResponse
        {
            Id = shipment.Id.Value.ToString(),
            OrderId = shipment.OrderId.Value.ToString(),
            DateDispatched = shipment.DateDispatched,
            DateDelivered = shipment.DateDelivered
        };
    }
}