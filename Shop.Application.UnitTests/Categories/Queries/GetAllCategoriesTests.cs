namespace Shop.Application.UnitTests.Categories.Queries;

using Domain.Entities;
using Application.Categories.Queries;

public class GetAllCategoriesTests
{
    private readonly TestDbContext _context;
    private readonly GetAllCategoriesHandler _handler;

    public GetAllCategoriesTests()
    {
        _context = new TestDbContext();
        _handler = new GetAllCategoriesHandler(_context);
    }

    [Fact]
    public async Task GetAllCategories_ShouldSucceed_WhenRequestValid()
    {
        var categories = new List<Category>
        {
            new
            (
                id: new CategoryId(Guid.NewGuid()),
                name: "category1"
            ),
            new
            (
                id: new CategoryId(Guid.NewGuid()),
                name: "category2"
            )
        };

        _context.Categories.AddRange(categories);
        
        await _context.SaveChangesAsync();

        var query = new GetAllCategoriesQuery();

        var result = await _handler.Handle(query, default);

        result.IsSuccess.Should().BeTrue();
        result.Value.Count.Should().Be(2);
    }
    
    [Fact]
    public async Task GetAllCategories_ShouldSucceed_WhenCategoriesEmpty()
    {
        var query = new GetAllCategoriesQuery();

        var result = await _handler.Handle(query, default);

        result.IsSuccess.Should().BeTrue();
        result.Value.Count.Should().Be(0);
    }
}