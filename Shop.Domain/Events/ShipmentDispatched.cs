namespace Shop.Domain.Events;

using Entities;
using Primitives;

public record ShipmentDispatched(Shipment Shipment) : DomainEvent;