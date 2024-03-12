namespace Shop.Application.Users.Commands;

public class LoginUserCommand : IRequest<Result<string>>
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}