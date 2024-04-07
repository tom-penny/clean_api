namespace Shop.Application.Addresses.Commands;

using Common.Interfaces;
using Domain.Entities;

public class CreateAddressCommand : IAuthorizedRequest<Result<Address>>
{
    public required Guid UserId { get; init; }
    public required string Street { get; init; }
    public required string City { get; init; }
    public required string Country { get; init; }
    public required string PostCode { get; init; }
}