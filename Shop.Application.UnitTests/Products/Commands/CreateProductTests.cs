namespace Shop.Application.UnitTests.Products.Commands;

using Application.Products.Commands;

public class CreateProductTests
{
    private readonly TestDbContext _context;
    private readonly CreateProductHandler _handler;
    private readonly CreateProductValidator _validator;

    public CreateProductTests()
    {
        _context = new TestDbContext();
        _handler = new CreateProductHandler(_context);
        _validator = new CreateProductValidator();
    }
    
    [Fact]
    public async Task CreateProduct_ShouldSucceed_WhenRequestValid()
    {
        var command = new CreateProductCommand
        {
            Name = "product",
            Stock = 10,
            Price = 50m,
            CategoryIds = new List<Guid>()
        };

        var result = await _handler.Handle(command, default);

        result.IsSuccess.Should().BeTrue();
        result.Value.Name.Should().Be("product");
    }
    
    [Fact]
    public void CreateProduct_ShouldReturnError_WhenNameEmpty()
    {
        var command = new CreateProductCommand
        {
            Name = "",
            Stock = 10,
            Price = 50m,
            CategoryIds = new List<Guid>()
        };
        
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Name);
    }
    
    [Fact]
    public void CreateProduct_ShouldReturnError_WhenStockLessThanZero()
    {
        var command = new CreateProductCommand
        {
            Name = "product",
            Stock = -1,
            Price = 50m,
            CategoryIds = new List<Guid>()
        };
        
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Stock);
    }
    
    [Fact]
    public void CreateProduct_ShouldReturnError_WhenPriceZero()
    {
        var command = new CreateProductCommand
        {
            Name = "product",
            Stock = 10,
            Price = decimal.Zero,
            CategoryIds = new List<Guid>()
        };
        
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Price);
    }
}