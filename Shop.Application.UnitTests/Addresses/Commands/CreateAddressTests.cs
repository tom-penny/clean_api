namespace Shop.Application.UnitTests.Addresses.Commands;

using Domain.Entities;
using Application.Addresses.Commands;

public class CreateAddressTests
{
    private readonly TestDbContext _context;
    private readonly CreateAddressHandler _handler;
    private readonly CreateAddressValidator _validator;
    
    public CreateAddressTests()
    {
        _context = new TestDbContext();
        _handler = new CreateAddressHandler(_context);
        _validator = new CreateAddressValidator();
    }
    
    [Fact]
    public async Task CreateAddress_ShouldSucceed_WhenRequestValid()
    {
        var userId = Guid.NewGuid();

        var user = new User
        (
            id: new UserId(userId),
            firstName: "firstName",
            lastName: "lastName",
            email: "test@test.com"
        );
        
        _context.Users.Add(user);
        
        await _context.SaveChangesAsync();
        
        var command = new CreateAddressCommand
        {
            UserId = userId,
            Street = "street",
            City = "city",
            Country = "country",
            PostCode = "postCode"
        };

        var result = await _handler.Handle(command, default);

        result.IsSuccess.Should().BeTrue();
        result.Value.UserId.Value.Should().Be(userId);
    }
    
    [Fact]
    public void CreateAddress_ShouldReturnError_WhenUserIdEmpty()
    {
        var command = new CreateAddressCommand
        {
            UserId = Guid.Empty,
            Street = "street",
            City = "city",
            Country = "country",
            PostCode = "postCode"
        };
        
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.UserId);
    }
    
    [Fact]
    public void CreateAddress_ShouldReturnError_WhenStreetEmpty()
    {
        var command = new CreateAddressCommand
        {
            UserId = Guid.NewGuid(),
            Street = "",
            City = "city",
            Country = "country",
            PostCode = "postCode"
        };
        
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Street);
    }
    
    [Fact]
    public void CreateAddress_ShouldReturnError_WhenCityEmpty()
    {
        var command = new CreateAddressCommand
        {
            UserId = Guid.NewGuid(),
            Street = "street",
            City = "",
            Country = "country",
            PostCode = "postCode"
        };
        
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.City);
    }
    
    [Fact]
    public void CreateAddress_ShouldReturnError_WhenCountryEmpty()
    {
        var command = new CreateAddressCommand
        {
            UserId = Guid.NewGuid(),
            Street = "street",
            City = "city",
            Country = "",
            PostCode = "postCode"
        };
        
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Country);
    }
    
    [Fact]
    public void CreateAddress_ShouldReturnError_WhenPostCodeEmpty()
    {
        var command = new CreateAddressCommand
        {
            UserId = Guid.NewGuid(),
            Street = "street",
            City = "city",
            Country = "country",
            PostCode = ""
        };
        
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.PostCode);
    }
}