namespace Shop.API.Mappings;

using Domain.Entities;
using Models.Responses;

public static class AddressMapping
{
    public static AddressResponse ToResponse(this Address address)
    {
        return new AddressResponse
        {
            Id = address.Id.Value.ToString(),
            UserId = address.UserId.Value.ToString(),
            Street = address.Street,
            City = address.City,
            Country = address.Country,
            PostCode = address.PostCode
        };
    }
    
    public static AddressesResponse ToResponse<T>(this IEnumerable<T> list) where T : Address
    {
        return new AddressesResponse
        {
            Addresses = list.Select(a => a.ToResponse()).ToList()
        };
    }
}