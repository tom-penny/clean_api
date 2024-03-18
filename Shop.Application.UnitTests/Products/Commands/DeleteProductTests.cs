namespace Shop.Application.UnitTests.Products.Commands;

using Domain.Entities;
using Application.Products.Commands;

public class DeleteProductTests
{
    private readonly TestDbContext _context;
    private readonly DeleteProductHandler _handler;
    private readonly DeleteProductValidator _validator;

    public DeleteProductTests()
    {
        _context = new TestDbContext();
        _handler = new DeleteProductHandler(_context);
        _validator = new DeleteProductValidator();
    }

    [Fact]
    public async Task DeleteProduct_ShouldSucceed_WhenRequestValid()
    {
        var productId = Guid.NewGuid();

        var product = new Product
        (
            id: new ProductId(productId),
            name: "product",
            stock: 10,
            price: 50m,
            categories: new List<Category>()
        );
        
        _context.Products.Add(product);
        
        await _context.SaveChangesAsync();

        var command = new DeleteProductCommand(productId);

        var result = await _handler.Handle(command, default);

        result.IsSuccess.Should().BeTrue();
    }
    
    [Fact]
    public async Task DeleteProduct_ShouldFail_WhenProductNotFound()
    {
        var command = new DeleteProductCommand(Guid.NewGuid());

        var result = await _handler.Handle(command, default);

        result.IsFailed.Should().BeTrue();
    }
    
    [Fact]
    public void DeleteProduct_ShouldReturnError_WhenIdEmpty()
    {
        var command = new DeleteProductCommand(Guid.Empty);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Id);
    }
}