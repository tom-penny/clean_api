namespace Shop.Domain.Entities;

using Primitives;

public record AddressId(Guid Value);

public class Address : BaseEntity
{
    public AddressId Id { get; init; }
    public UserId UserId { get; init; }
    public string Street { get; init; }
    public string City { get; init; }
    public string Country { get; init; }
    public string PostCode { get; init; }
    public bool IsActive { get; private set; }
    
    public Address(AddressId id, UserId userId, string street, string city, string country, string postCode)
    {
        Id = id;
        UserId = userId;
        Street = street;
        City = city;
        Country = country;
        PostCode = postCode;
        IsActive = true;
    }
    
    public void Deactivate()
    {
        IsActive = false;
    }
    
    private Address() {}
}