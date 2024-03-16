using Microsoft.Extensions.Logging;

namespace Shop.Application.Orders.Events;

using Common.Interfaces;
using Domain.Events;

public class OrderCancelledHandler : INotificationHandler<OrderCancelled>
{
    private readonly ILogger<OrderConfirmedHandler> _logger;
    private readonly IEmailService _emailService;

    public OrderCancelledHandler(ILogger<OrderConfirmedHandler> logger, IEmailService emailService)
    {
        _logger = logger;
        _emailService = emailService;
    }

    public async Task Handle(OrderCancelled notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{@DateTime}: Started {@Request}",
            nameof(OrderCancelledHandler), DateTime.UtcNow);
        
        
        
        _logger.LogInformation("{@DateTime}: Completed {@Request}",
            nameof(OrderCancelledHandler), DateTime.UtcNow);
    }
}