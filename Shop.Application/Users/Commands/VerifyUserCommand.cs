namespace Shop.Application.Users.Commands;

public class VerifyUserCommand : IRequest<Result<Unit>>
{
    public required Guid Id { get; init; }
    public required string Token { get; init; }
}