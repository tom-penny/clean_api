namespace Shop.Application.Addresses.Queries;

using Common.Interfaces;
using Domain.Entities;

public class GetAllAddressesQuery : IAuthorizedRequest<Result<List<Address>>>
{
    public required Guid UserId { get; init; }
}