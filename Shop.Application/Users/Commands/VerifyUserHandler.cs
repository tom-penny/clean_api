namespace Shop.Application.Users.Commands;

using Common.Interfaces;
using Domain.Errors;

public class VerifyUserHandler : IRequestHandler<VerifyUserCommand, Result<Unit>>
{
    private readonly IUserService _userService;

    public VerifyUserHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<Result<Unit>> Handle(VerifyUserCommand request, CancellationToken cancellationToken)
    {
        var result = await _userService.ValidateConfirmationTokenAsync(request.Id, request.Token);

        if (!result.IsSuccess) return Result.Fail(UserError.InvalidToken());

        await _userService.AddRoleAsync(request.Id, "User");

        return Result.Ok();
    }
}