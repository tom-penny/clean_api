using Microsoft.Extensions.Logging;

namespace Shop.Application.Orders.Events;

using Interfaces;
using Domain.Events;

public class OrderConfirmedHandler : INotificationHandler<OrderConfirmed>
{
    private readonly ILogger<OrderConfirmedHandler> _logger;
    private readonly IEmailService _emailService;

    public OrderConfirmedHandler(ILogger<OrderConfirmedHandler> logger, IEmailService emailService)
    {
        _logger = logger;
        _emailService = emailService;
    }

    public async Task Handle(OrderConfirmed notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{@DateTime}: Started {@Request}",
            nameof(OrderConfirmedHandler), DateTime.UtcNow);

        const string email = "";

        const string body = "";

        await _emailService.SendEmailAsync(email, $"Order {notification.Order.Id} Confirmed", body, cancellationToken);
        
        _logger.LogInformation("{@DateTime}: Completed {@Request}",
            nameof(OrderConfirmedHandler), DateTime.UtcNow);
    }
}