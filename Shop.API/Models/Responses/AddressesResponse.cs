namespace Shop.API.Models.Responses;

public class AddressesResponse
{
    public required List<AddressResponse> Addresses { get; init; }
}