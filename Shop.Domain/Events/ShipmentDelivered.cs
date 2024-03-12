namespace Shop.Domain.Events;

using Entities;
using Primitives;

public record ShipmentDelivered(Shipment Shipment) : DomainEvent;