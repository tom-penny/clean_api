namespace Shop.Application.Addresses.Commands;

using Domain.Entities;

public class CreateAddressCommand : IRequest<Result<Address>>
{
    public required Guid UserId { get; init; }
    public required string Street { get; init; }
    public required string City { get; init; }
    public required string Country { get; init; }
    public required string PostCode { get; init; }
}