namespace Shop.Application.Addresses.Queries;

using Common.Interfaces;
using Domain.Entities;

public record GetAllAddressesQuery(Guid UserId) : IAuthorizedRequest<Result<List<Address>>>;