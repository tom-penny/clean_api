namespace Shop.Application.Addresses.Commands;

using Common.Interfaces;

public record DeleteAddressCommand(Guid Id, Guid UserId) : IAuthorizedRequest<Result<Unit>>;