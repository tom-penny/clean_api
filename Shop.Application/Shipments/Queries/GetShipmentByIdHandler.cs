namespace Shop.Application.Shipments.Queries;

using Common.Interfaces;
using Domain.Entities;
using Domain.Errors;

public class GetShipmentByIdHandler : IRequestHandler<GetShipmentByIdQuery, Result<Shipment>>
{
    private readonly IApplicationDbContext _context;

    public GetShipmentByIdHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Shipment>> Handle(GetShipmentByIdQuery request, CancellationToken cancellationToken)
    {
        var shipment = await _context.Shipments.FirstOrDefaultAsync(s =>
            s.Id == new ShipmentId(request.Id), cancellationToken);

        return shipment != null ? Result.Ok(shipment) : Result.Fail(ShipmentError.NotFound(request.Id));
    }
}