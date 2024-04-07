namespace Shop.Application.Addresses.Queries;

using Common.Interfaces;
using Domain.Entities;

public record GetAddressByIdQuery(Guid Id, Guid UserId) : IAuthorizedRequest<Result<Address>>;