namespace Shop.Application.Shipments.Commands;

using Interfaces;
using Domain.Entities;
using Domain.Errors;

public class CreateShipmentHandler : IRequestHandler<CreateShipmentCommand, Result<Shipment>>
{
    private readonly IApplicationDbContext _context;

    public CreateShipmentHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Result<Shipment>> Handle(CreateShipmentCommand request, CancellationToken cancellationToken)
    {
        var orderExists = await _context.Orders.AnyAsync(o =>
            o.Id == new OrderId(request.OrderId), cancellationToken);

        if (!orderExists) return Result.Fail(ShipmentError.OrderNotFound(request.OrderId));

        var shipment = new Shipment
        (
            id: new ShipmentId(Guid.NewGuid()),
            orderId: new OrderId(request.OrderId)
        );
        
        shipment.SetDispatchDate(DateTime.UtcNow);

        _context.Shipments.Add(shipment);

        await _context.SaveChangesAsync(cancellationToken);
        
        return Result.Ok(shipment);
    }
}