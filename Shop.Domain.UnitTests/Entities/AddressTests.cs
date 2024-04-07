namespace Shop.Domain.UnitTests.Entities;

using Domain.Entities;

public class AddressTests
{
    private readonly Address _address;

    public AddressTests()
    {
        _address = new Address
        (
            id: new AddressId(Guid.NewGuid()),
            userId: new UserId(Guid.NewGuid()),
            street: "street",
            city: "city",
            country: "country",
            postCode: "postCode"
        );
    }
    
    [Fact]
    public void Deactivate_ShouldSetActiveToFalse()
    {
        _address.Deactivate();

        _address.IsActive.Should().BeFalse();
    }
}