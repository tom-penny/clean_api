namespace Shop.API.Models.Responses;

public class AddressResponse
{
    public required string Id { get; init; }
    public required string UserId { get; init; }
    public required string Street { get; init; }
    public required string City { get; init; }
    public required string Country { get; init; }
    public required string PostCode { get; init; }
}