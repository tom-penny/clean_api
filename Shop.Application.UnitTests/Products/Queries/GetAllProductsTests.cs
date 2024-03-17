namespace Shop.Application.UnitTests.Products.Queries;

using Domain.Entities;
using Application.Products.Queries;

public class GetAllProductsTests
{
    private readonly TestDbContext _context;
    private readonly GetAllProductsHandler _handler;
    private readonly GetAllProductsValidator _validator;

    public GetAllProductsTests()
    {
        _context = new TestDbContext();
        _handler = new GetAllProductsHandler(_context);
        _validator = new GetAllProductsValidator();
    }

    [Fact]
    public async Task GetAllProducts_ShouldSucceed_WhenRequestValid()
    {
        var products = new List<Product>
        {
            new
            (
                id: new ProductId(Guid.NewGuid()),
                name: "product1",
                stock: 10,
                price: 50m,
                new List<Category>()
            ),
            new
            (
                id: new ProductId(Guid.NewGuid()),
                name: "product2",
                stock: 10,
                price: 50m,
                new List<Category>()
            )
        };

        _context.Products.AddRange(products);

        await _context.SaveChangesAsync();

        var query = new GetAllProductsQuery
        {
            SortBy = "name",
            OrderBy = "asc",
            Page = 1,
            Size = 10
        };

        var result = await _handler.Handle(query, default);

        result.IsSuccess.Should().BeTrue();
        result.Value.Items.Count.Should().Be(2);
    }

    [Fact]
    public void GetAllProducts_ShouldReturnError_WhenSortByInvalid()
    {
        var query = new GetAllProductsQuery
        {
            SortBy = "invalid",
            OrderBy = "asc",
            Page = 1,
            Size = 10
        };

        var result = _validator.TestValidate(query);
        
        result.ShouldHaveValidationErrorFor(q => q.SortBy);
    }
    
    [Fact]
    public void GetAllProducts_ShouldReturnError_WhenOrderByInvalid()
    {
        var query = new GetAllProductsQuery
        {
            SortBy = "name",
            OrderBy = "invalid",
            Page = 1,
            Size = 10
        };

        var result = _validator.TestValidate(query);
        
        result.ShouldHaveValidationErrorFor(q => q.OrderBy);
    }
    
    [Fact]
    public void GetAllProducts_ShouldReturnError_WhenPageZero()
    {
        var query = new GetAllProductsQuery
        {
            SortBy = "name",
            OrderBy = "asc",
            Page = 0,
            Size = 10
        };

        var result = _validator.TestValidate(query);
        
        result.ShouldHaveValidationErrorFor(q => q.Page);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(26)]
    public void GetAllProducts_ShouldReturnError_WhenSizeInvalid(int size)
    {
        var query = new GetAllProductsQuery
        {
            SortBy = "name",
            OrderBy = "asc",
            Page = 1,
            Size = size
        };

        var result = _validator.TestValidate(query);
        
        result.ShouldHaveValidationErrorFor(q => q.Size);
    }
}