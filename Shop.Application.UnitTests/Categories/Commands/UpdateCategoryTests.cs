namespace Shop.Application.UnitTests.Categories.Commands;

using Domain.Entities;
using Application.Categories.Commands;

public class UpdateCategoryTests
{
    private readonly TestDbContext _context;
    private readonly UpdateCategoryHandler _handler;
    private readonly UpdateCategoryValidator _validator;
    
    public UpdateCategoryTests()
    {
        _context = new TestDbContext();
        _handler = new UpdateCategoryHandler(_context);
        _validator = new UpdateCategoryValidator();
    }

    [Fact]
    public async Task UpdateCategory_ShouldSucceed_WhenRequestValid()
    {
        var categoryId = Guid.NewGuid();

        var category = new Category
        (
            id: new CategoryId(categoryId),
            name: "category1"
        );
        
        _context.Categories.Add(category);
        
        await _context.SaveChangesAsync();

        var command = new UpdateCategoryCommand
        {
            Id = categoryId,
            Name = "category2"
        };

        var result = await _handler.Handle(command, default);

        result.IsSuccess.Should().BeTrue();
        category.Name.Should().Be("category2");
    }
    
    [Fact]
    public async Task UpdateCategory_ShouldFail_WhenCategoryNotFound()
    {
        var command = new UpdateCategoryCommand
        {
            Id = Guid.NewGuid(),
            Name = "category"
        };

        var result = await _handler.Handle(command, default);

        result.IsFailed.Should().BeTrue();
    }
    
    [Fact]
    public async Task UpdateCategory_ShouldFail_WhenNameExists()
    {
        var categoryId = Guid.NewGuid();

        var category = new Category
        (
            id: new CategoryId(categoryId),
            name: "category"
        );
        
        _context.Categories.Add(category);
        
        await _context.SaveChangesAsync();

        var command = new UpdateCategoryCommand
        {
            Id = categoryId,
            Name = "category"
        };

        var result = await _handler.Handle(command, default);

        result.IsFailed.Should().BeTrue();
    }
    
    [Fact]
    public void UpdateCategory_ShouldReturnError_WhenIdEmpty()
    {
        var command = new UpdateCategoryCommand
        {
            Id = Guid.Empty,
            Name = "category"
        };
        
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Id);
    }
    
    [Fact]
    public void UpdateCategory_ShouldReturnError_WhenNameEmpty()
    {
        var command = new UpdateCategoryCommand
        {
            Id = Guid.NewGuid(),
            Name = ""
        };
        
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Name);
    }
}