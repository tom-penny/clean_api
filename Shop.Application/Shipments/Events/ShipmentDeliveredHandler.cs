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

        try
        {
            var order = await _context.Orders.FirstAsync(o =>
                o.Id == notification.Shipment.OrderId, cancellationToken);

            order.SetCompleted();

            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "{@DateTime}: Failed {@Request}",
                nameof(ShipmentDeliveredHandler), DateTime.UtcNow);
        }
        
        _logger.LogInformation("{@DateTime}: Completed {@Request}",
            nameof(ShipmentDeliveredHandler), DateTime.UtcNow);
    }
}