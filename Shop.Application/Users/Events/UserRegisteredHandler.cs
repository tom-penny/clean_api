using Microsoft.Extensions.Logging;

namespace Shop.Application.Users.Events;

using Common.Interfaces;
using Domain.Events;

public class UserRegisteredHandler : INotificationHandler<UserRegistered>
{
    private readonly ILogger<UserRegisteredHandler> _logger;
    private readonly IUserService _userService;
    private readonly IEmailService _emailService;

    public UserRegisteredHandler(ILogger<UserRegisteredHandler> logger,
        IUserService userService, IEmailService emailService)
    {
        _logger = logger;
        _userService = userService;
        _emailService = emailService;
    }

    public async Task Handle(UserRegistered notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{@DateTime}: Started {@Request}",
            nameof(UserRegisteredHandler), DateTime.UtcNow);

        try
        {
            var link = await _userService.GenerateConfirmationTokenAsync(notification.User.Id.Value);

            var message = $"Click here to verify: {link}";

            await _emailService.SendEmailAsync(notification.User.Email, "subject", message, cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "{@DateTime}: Failed {@Request}",
                nameof(UserRegisteredHandler), DateTime.UtcNow);
        }

        _logger.LogInformation("{@DateTime}: Completed {@Request}",
            nameof(UserRegisteredHandler), DateTime.UtcNow);
    }
}