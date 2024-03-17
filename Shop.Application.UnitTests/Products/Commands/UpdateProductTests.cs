namespace Shop.Application.UnitTests.Products.Commands;

using Domain.Entities;
using Application.Products.Commands;

public class UpdateProductTests
{
    private readonly TestDbContext _context;
    private readonly UpdateProductHandler _handler;
    private readonly UpdateProductValidator _validator;

    public UpdateProductTests()
    {
        _context = new TestDbContext();
        _handler = new UpdateProductHandler(_context);
        _validator = new UpdateProductValidator();
    }
    
    [Fact]
    public async Task UpdateProduct_ShouldSucceed_WhenRequestValid()
    {
        var productId = Guid.NewGuid();

        var product = new Shop.Domain.Entities.Product
        (
            id: new ProductId(productId),
            name: "product1",
            stock: 10,
            price: 50m,
            categories: new List<Category>()
        );
        
        _context.Products.Add(product);
        
        await _context.SaveChangesAsync();
        
        var command = new UpdateProductCommand
        {
            Id = productId,
            Name = "product2",
            Stock = 20,
            Price = 40m,
            CategoryIds = new List<Guid>()
        };

        var result = await _handler.Handle(command, default);

        result.IsSuccess.Should().BeTrue();
        product.Name.Should().Be("product2");
    }
    
    [Fact]
    public void UpdateProduct_ShouldReturnError_WhenIdEmpty()
    {
        var command = new UpdateProductCommand
        {
            Id = Guid.Empty,
            Name = "product",
            Stock = 10,
            Price = 50m,
            CategoryIds = new List<Guid>()
        };
        
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Id);
    }
    
    [Fact]
    public void UpdateProduct_ShouldReturnError_WhenNameEmpty()
    {
        var command = new UpdateProductCommand
        {
            Id = Guid.NewGuid(),
            Name = "",
            Stock = 10,
            Price = 50m,
            CategoryIds = new List<Guid>()
        };
        
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Name);
    }
    
    [Fact]
    public void UpdateProduct_ShouldReturnError_WhenStockLessThanZero()
    {
        var command = new UpdateProductCommand
        {
            Id = Guid.NewGuid(),
            Name = "product",
            Stock = -1,
            Price = 50m,
            CategoryIds = new List<Guid>()
        };
        
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Stock);
    }
    
    [Fact]
    public void UpdateProduct_ShouldReturnError_WhenPriceZero()
    {
        var command = new UpdateProductCommand
        {
            Id = Guid.NewGuid(),
            Name = "product",
            Stock = 10,
            Price = decimal.Zero,
            CategoryIds = new List<Guid>()
        };
        
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Price);
    }
}