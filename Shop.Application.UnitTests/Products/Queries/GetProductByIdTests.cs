namespace Shop.Application.UnitTests.Products.Queries;

using Domain.Entities;
using Application.Products.Queries;

public class GetProductByIdTests
{
    private readonly TestDbContext _context;
    private readonly GetProductByIdHandler _handler;
    private readonly GetProductByIdValidator _validator;

    public GetProductByIdTests()
    {
        _context = new TestDbContext();
        _handler = new GetProductByIdHandler(_context);
        _validator = new GetProductByIdValidator();
    }

    [Fact]
    public async Task GetProductById_ShouldSucceed_WhenRequestValid()
    {
        var productId = Guid.NewGuid();

        var product1 = new Product
        (
            id: new ProductId(productId),
            name: "product1",
            stock: 10,
            price: 50m,
            new List<Category>()
        );
        
        var product2 = new Product
        (
            id: new ProductId(Guid.NewGuid()),
            name: "product2",
            stock: 10,
            price: 50m,
            new List<Category>()
        );
        
        _context.Products.AddRange(new List<Product> { product1, product2 });
        
        await _context.SaveChangesAsync();

        var query = new GetProductByIdQuery { Id = productId };

        var result = await _handler.Handle(query, default);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeSameAs(product1);
    }

    [Fact]
    public async Task GetProductById_ShouldFail_WhenProductNotFound()
    {
        var product1 = new Product
        (
            id: new ProductId(Guid.NewGuid()),
            name: "product1",
            stock: 10,
            price: 50m,
            new List<Category>()
        );
        
        var product2 = new Product
        (
            id: new ProductId(Guid.NewGuid()),
            name: "product2",
            stock: 10,
            price: 50m,
            new List<Category>()
        );
        
        _context.Products.AddRange(new List<Product> { product1, product2 });
        
        await _context.SaveChangesAsync();

        var query = new GetProductByIdQuery { Id = Guid.NewGuid() };

        var result = await _handler.Handle(query, default);

        result.IsFailed.Should().BeTrue();
    }

    [Fact]
    public void GetProductById_ShouldReturnError_WhenIdEmpty()
    {
        var query = new GetProductByIdQuery { Id = Guid.Empty };
        var result = _validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(q => q.Id);
    }
}