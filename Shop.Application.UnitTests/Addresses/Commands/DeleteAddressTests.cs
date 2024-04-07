namespace Shop.Application.UnitTests.Addresses.Commands;

using Domain.Entities;
using Application.Addresses.Commands;

public class DeleteAddressTests
{
    private readonly TestDbContext _context;
    private readonly DeleteAddressHandler _handler;
    private readonly DeleteAddressValidator _validator;
    
    public DeleteAddressTests()
    {
        _context = new TestDbContext();
        _handler = new DeleteAddressHandler(_context);
        _validator = new DeleteAddressValidator();
    }
    
    [Fact]
    public async Task DeleteAddress_ShouldSucceed_WhenRequestValid()
    {
        var addressId = Guid.NewGuid();

        var userId = Guid.NewGuid();

        var address = new Address
        (
            id: new AddressId(addressId),
            userId: new UserId(Guid.NewGuid()),
            street: "street",
            city: "city",
            country: "country",
            postCode: "postCode"
        );
        
        _context.Addresses.Add(address);
        
        await _context.SaveChangesAsync();

        var command = new DeleteAddressCommand(addressId, userId);

        var result = await _handler.Handle(command, default);

        result.IsSuccess.Should().BeTrue();
    }
    
    [Fact]
    public async Task DeleteAddress_ShouldFail_WhenAddressNotFound()
    {
        var command = new DeleteAddressCommand(Guid.NewGuid(), Guid.NewGuid());

        var result = await _handler.Handle(command, default);

        result.IsFailed.Should().BeTrue();
    }
    
    [Fact]
    public void DeleteAddress_ShouldReturnError_WhenIdEmpty()
    {
        var command = new DeleteAddressCommand(Guid.Empty, Guid.NewGuid());

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Id);
    }
    
    [Fact]
    public void DeleteAddress_ShouldReturnError_WhenUserIdEmpty()
    {
        var command = new DeleteAddressCommand(Guid.NewGuid(), Guid.Empty);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.UserId);
    }
}