namespace Shop.Application.UnitTests.Addresses.Queries;

using Domain.Entities;
using Application.Addresses.Queries;

public class GetAddressByIdTests
{
    private readonly TestDbContext _context;
    private readonly GetAddressByIdHandler _handler;
    private readonly GetAddressByIdValidator _validator;

    public GetAddressByIdTests()
    {
        _context = new TestDbContext();
        _handler = new GetAddressByIdHandler(_context);
        _validator = new GetAddressByIdValidator();
    }
    
    [Fact]
    public async Task GetAddressById_ShouldSucceed_WhenRequestValid()
    {
        var addressId = Guid.NewGuid();
        
        var userId = Guid.NewGuid();

        var address1 = new Address
        (
            id: new AddressId(addressId),
            userId: new UserId(userId),
            street: "street1",
            city: "city1",
            country: "country1",
            postCode: "postCode1"
        );

        var address2 = new Address
        (
            id: new AddressId(Guid.NewGuid()),
            userId: new UserId(userId),
            street: "street2",
            city: "city2",
            country: "country2",
            postCode: "postCode2"
        );

        _context.Addresses.AddRange(new List<Address> { address1, address2 });

        await _context.SaveChangesAsync();
        
        var query = new GetAddressByIdQuery(addressId, userId);

        var result = await _handler.Handle(query, default);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeSameAs(address1);
    }
    
    [Fact]
    public async Task GetAddressById_ShouldFail_WhenAddressNotFound()
    {
        var userId = Guid.NewGuid();

        var address1 = new Address
        (
            id: new AddressId(Guid.NewGuid()),
            userId: new UserId(userId),
            street: "street1",
            city: "city1",
            country: "country1",
            postCode: "postCode1"
        );

        var address2 = new Address
        (
            id: new AddressId(Guid.NewGuid()),
            userId: new UserId(userId),
            street: "street2",
            city: "city2",
            country: "country2",
            postCode: "postCode2"
        );
    
        _context.Addresses.AddRange(new List<Address> { address1, address2 });

        await _context.SaveChangesAsync();
        
        var query = new GetAddressByIdQuery(Guid.NewGuid(), userId);
    
        var result = await _handler.Handle(query, default);
    
        result.IsFailed.Should().BeTrue();
    }
    
    [Fact]
    public void GetAddressById_ShouldReturnError_WhenAddressIdEmpty()
    {
        var query = new GetAddressByIdQuery(Guid.Empty, Guid.NewGuid());
        
        var result = _validator.TestValidate(query);
    
        result.ShouldHaveValidationErrorFor(q => q.Id);
    }
    
    [Fact]
    public void GetAddressById_ShouldReturnError_WhenUserIdEmpty()
    {
        var query = new GetAddressByIdQuery(Guid.NewGuid(), Guid.Empty);
        
        var result = _validator.TestValidate(query);
    
        result.ShouldHaveValidationErrorFor(q => q.UserId);
    }
}