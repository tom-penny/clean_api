namespace Shop.Application.Users.Events;

using Common.Interfaces;
using Domain.Events;

public class UserRegisteredHandler : INotificationHandler<UserRegistered>
{
    private readonly IUserService _userService;
    private readonly IEmailService _emailService;

    public UserRegisteredHandler(IUserService userService, IEmailService emailService)
    {
        _userService = userService;
        _emailService = emailService;
    }

    public async Task Handle(UserRegistered notification, CancellationToken cancellationToken)
    {
        var link = await _userService.GenerateConfirmationTokenAsync(notification.User.Id.Value);

        var message = $"Click here to verify: {link}";

        await _emailService.SendEmailAsync(notification.User.Email, "subject", message, cancellationToken);
    }
}