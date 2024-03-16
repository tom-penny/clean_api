using Microsoft.Extensions.Logging;

namespace Shop.Application.Shipments.Events;

using Common.Interfaces;
using Domain.Events;

// Domain event handler: sets order to completed.

public class ShipmentDeliveredHandler : INotificationHandler<ShipmentDelivered>
{
    private readonly ILogger<ShipmentDeliveredHandler> _logger;
    private readonly IApplicationDbContext _context;

    public ShipmentDeliveredHandler(ILogger<ShipmentDeliveredHandler> logger, IApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task Handle(ShipmentDelivered notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{@DateTime}: Started {@Request}",
            nameof(ShipmentDeliveredHandler), DateTime.UtcNow);
        
        var order = await _context.Orders.FirstOrDefaultAsync(o =>
            o.Id == notification.Shipment.OrderId, cancellationToken);
        
        if (order == null) throw new Exception();

        order.SetCompleted();
                
        await _context.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("{@DateTime}: Completed {@Request}",
            nameof(ShipmentDeliveredHandler), DateTime.UtcNow);
    }
}