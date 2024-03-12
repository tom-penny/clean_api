namespace Shop.Application.Shipments.Queries;

using Domain.Entities;

public record GetShipmentByIdQuery(Guid Id) : IRequest<Result<Shipment>>;