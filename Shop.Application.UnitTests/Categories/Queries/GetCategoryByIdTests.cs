namespace Shop.Application.UnitTests.Categories.Queries;

using Domain.Entities;
using Application.Categories.Queries;

public class GetCategoryByIdTests
{
    private readonly TestDbContext _context;
    private readonly GetCategoryByIdHandler _handler;
    private readonly GetCategoryByIdValidator _validator;

    public GetCategoryByIdTests()
    {
        _context = new TestDbContext();
        _handler = new GetCategoryByIdHandler(_context);
        _validator = new GetCategoryByIdValidator();
    }
    
    [Fact]
    public async Task GetCategoryById_ShouldSucceed_WhenRequestValid()
    {
        var categoryId = Guid.NewGuid();

        var category1 = new Category
        (
            id: new CategoryId(categoryId),
            name: "category1"
        );
        
        var category2 = new Category
        (
            id: new CategoryId(Guid.NewGuid()),
            name: "category2"
        );
        
        _context.Categories.AddRange(new List<Category> { category1, category2 });
        
        await _context.SaveChangesAsync();

        var query = new GetCategoryByIdQuery { Id = categoryId };

        var result = await _handler.Handle(query, default);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeSameAs(category1);
    }

    [Fact]
    public async Task GetCategoryById_ShouldFail_WhenCategoryNotFound()
    {
        var category1 = new Category
        (
            id: new CategoryId(Guid.NewGuid()),
            name: "category1"
        );
        
        var category2 = new Category
        (
            id: new CategoryId(Guid.NewGuid()),
            name: "category2"
        );
        
        _context.Categories.AddRange(new List<Category> { category1, category2 });
        
        await _context.SaveChangesAsync();

        var query = new GetCategoryByIdQuery { Id = Guid.NewGuid() };

        var result = await _handler.Handle(query, default);
        
        result.IsFailed.Should().BeTrue();
    }
    
    [Fact]
    public void GetCategoryById_ShouldReturnError_WhenIdEmpty()
    {
        var query = new GetCategoryByIdQuery { Id = Guid.Empty };
        
        var result = _validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(q => q.Id);
    }
}