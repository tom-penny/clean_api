namespace Shop.Application.Addresses.Commands;

public record DeleteAddressCommand(Guid Id) : IRequest<Result<Unit>>;