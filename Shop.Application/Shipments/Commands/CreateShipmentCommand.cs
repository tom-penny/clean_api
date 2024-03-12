namespace Shop.Application.Shipments.Commands;

using Domain.Entities;

public record CreateShipmentCommand(Guid OrderId) : IRequest<Result<Shipment>>;