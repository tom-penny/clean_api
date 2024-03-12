namespace Shop.Application.Users.Commands;

using Interfaces;
using Domain.Errors;

public class LoginUserHandler : IRequestHandler<LoginUserCommand, Result<string>>
{
    private readonly IUserService _userService;
    private readonly IJwtProvider _jwtProvider;

    public LoginUserHandler(IUserService userService, IJwtProvider jwtProvider)
    {
        _userService = userService;
        _jwtProvider = jwtProvider;
    }

    public async Task<Result<string>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var loginResult = await _userService.SignInUserAsync(request.Email, request.Password);

        if (loginResult.IsFailed) return Result.Fail(UserError.InvalidCredentials());

        var roleResult = await _userService.GetRolesByIdAsync(loginResult.Value);
        
        var token = _jwtProvider.GenerateToken(loginResult.Value, request.Email, roleResult.Value);

        return Result.Ok(token);
    }
}