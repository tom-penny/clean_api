namespace Shop.Application.Users.Commands;

public record LogoutUserCommand() : IRequest<Result<Unit>>;