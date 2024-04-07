namespace Shop.Application.UnitTests.Addresses.Queries;

using Domain.Entities;
using Application.Addresses.Queries;

public class GetAllAddressesTests
{
    private readonly TestDbContext _context;
    private readonly GetAllAddressesHandler _handler;
    private readonly GetAllAddressesValidator _validator;

    public GetAllAddressesTests()
    {
        _context = new TestDbContext();
        _handler = new GetAllAddressesHandler(_context);
        _validator = new GetAllAddressesValidator();
    }
    
    [Fact]
    public async Task GetAllAddresses_ShouldSucceed_WhenRequestValid()
    {
        var userId = Guid.NewGuid();
        
        var addresses = new List<Address>
        {
            new
            (
                id: new AddressId(Guid.NewGuid()),
                userId: new UserId(userId),
                street: "street1",
                city: "city1",
                country: "country1",
                postCode: "postCode1"
            ),
            new
            (
                id: new AddressId(Guid.NewGuid()),
                userId: new UserId(userId),
                street: "street2",
                city: "city2",
                country: "country2",
                postCode: "postCode2"
            )
        };
        
        _context.Addresses.AddRange(addresses);
        
        await _context.SaveChangesAsync();

        var query = new GetAllAddressesQuery(userId);

        var result = await _handler.Handle(query, default);

        result.IsSuccess.Should().BeTrue();
        result.Value.Count.Should().Be(2);
    }
    
    [Fact]
    public async Task GetAllAddresses_ShouldSucceed_WhenAddressesEmpty()
    {
        var query = new GetAllAddressesQuery(Guid.NewGuid());
        
        var result = await _handler.Handle(query, default);

        result.IsSuccess.Should().BeTrue();
        result.Value.Count.Should().Be(0);
    }
    
    [Fact]
    public void GetAllAddresses_ShouldReturnError_WhenUserIdEmpty()
    {
        var query = new GetAllAddressesQuery(Guid.Empty);
        
        var result = _validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(q => q.UserId);
    }
}