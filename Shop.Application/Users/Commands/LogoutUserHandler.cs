namespace Shop.Application.Users.Commands;

using Interfaces;

public class LogoutUserHandler : IRequestHandler<LogoutUserCommand, Result<Unit>>
{
    private readonly IUserService _userService;

    public LogoutUserHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<Result<Unit>> Handle(LogoutUserCommand request, CancellationToken cancellationToken)
    {
        await _userService.SignOutUserAsync();

        return Result.Ok();
    }
}