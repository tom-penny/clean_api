namespace Shop.API.Models.Requests;

public class CreateAddressRequest
{
    public required string Street { get; init; }
    public required string City { get; init; }
    public required string Country { get; init; }
    public required string PostCode { get; init; }
}