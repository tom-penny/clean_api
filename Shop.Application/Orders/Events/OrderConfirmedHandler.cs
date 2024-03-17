using Microsoft.Extensions.Logging;

namespace Shop.Application.Orders.Events;

using Common.Interfaces;
using Domain.Events;

public class OrderConfirmedHandler : INotificationHandler<OrderConfirmed>
{
    private readonly ILogger<OrderConfirmedHandler> _logger;
    private readonly IApplicationDbContext _context;
    private readonly IEmailService _emailService;

    public OrderConfirmedHandler(ILogger<OrderConfirmedHandler> logger,
        IApplicationDbContext context, IEmailService emailService)
    {
        _logger = logger;
        _context = context;
        _emailService = emailService;
    }

    public async Task Handle(OrderConfirmed notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{@DateTime}: Started {@Request}",
            nameof(OrderConfirmedHandler), DateTime.UtcNow);

        try
        {
            var user = await _context.Users.FirstAsync(u =>
                u.Id == notification.Order.UserId, cancellationToken);
            
            var subject = $"Order {notification.Order.Id} Confirmed";

            var body = "Thanks for your order.";
            
            await _emailService.SendEmailAsync(user.Email, subject, body, cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "{@DateTime}: Failed {@Request}",
                nameof(OrderConfirmedHandler), DateTime.UtcNow);
        }
        
        _logger.LogInformation("{@DateTime}: Completed {@Request}",
            nameof(OrderConfirmedHandler), DateTime.UtcNow);
    }
}