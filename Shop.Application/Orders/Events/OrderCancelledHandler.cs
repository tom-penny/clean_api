using Microsoft.Extensions.Logging;

namespace Shop.Application.Orders.Events;

using Common.Interfaces;
using Domain.Events;

public class OrderCancelledHandler : INotificationHandler<OrderCancelled>
{
    private readonly ILogger<OrderConfirmedHandler> _logger;
    private readonly IApplicationDbContext _context;
    private readonly IEmailService _emailService;

    public OrderCancelledHandler(ILogger<OrderConfirmedHandler> logger,
        IApplicationDbContext context, IEmailService emailService)
    {
        _logger = logger;
        _context = context;
        _emailService = emailService;
    }

    public async Task Handle(OrderCancelled notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{@DateTime}: Started {@Request}",
            nameof(OrderCancelledHandler), DateTime.UtcNow);

        try
        {
            var user = await _context.Users.FirstAsync(u =>
                u.Id == notification.Order.UserId, cancellationToken);
            
            var subject = $"Order {notification.Order.Id} Cancelled";

            var body = "There was a problem with your order.";
            
            await _emailService.SendEmailAsync(user.Email, subject , body, cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "{@DateTime}: Failed {@Request}",
                nameof(OrderCancelledHandler), DateTime.UtcNow);
        }
        
        _logger.LogInformation("{@DateTime}: Completed {@Request}",
            nameof(OrderCancelledHandler), DateTime.UtcNow);
    }
}