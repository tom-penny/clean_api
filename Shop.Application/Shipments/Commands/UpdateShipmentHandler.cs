namespace Shop.Application.Shipments.Commands;

using Interfaces;
using Domain.Entities;
using Domain.Errors;

public class UpdateShipmentHandler : IRequestHandler<UpdateShipmentCommand, Result<Unit>>
{
    private readonly IApplicationDbContext _context;

    public UpdateShipmentHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Unit>> Handle(UpdateShipmentCommand request, CancellationToken cancellationToken)
    {
        var shipment = await _context.Shipments.FirstOrDefaultAsync(s =>
            s.Id == new ShipmentId(request.Id), cancellationToken);

        if (shipment == null) return Result.Fail(ShipmentError.NotFound(request.Id));

        shipment.SetDeliveryDate(request.DeliveryDate);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}