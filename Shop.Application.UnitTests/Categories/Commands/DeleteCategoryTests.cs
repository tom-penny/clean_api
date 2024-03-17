namespace Shop.Application.UnitTests.Categories.Commands;

using Domain.Entities;
using Application.Categories.Commands;

public class DeleteCategoryTests
{
    private readonly TestDbContext _context;
    private readonly DeleteCategoryHandler _handler;
    private readonly DeleteCategoryValidator _validator;

    public DeleteCategoryTests()
    {
        _context = new TestDbContext();
        _handler = new DeleteCategoryHandler(_context);
        _validator = new DeleteCategoryValidator();
    }

    [Fact]
    public async Task DeleteCategory_ShouldSucceed_WhenRequestValid()
    {
        var categoryId = Guid.NewGuid();

        var category = new Category
        (
            id: new CategoryId(categoryId),
            name: "category"
        );
        
        _context.Categories.Add(category);
        
        await _context.SaveChangesAsync();

        var command = new DeleteCategoryCommand(categoryId);

        var result = await _handler.Handle(command, default);

        result.IsSuccess.Should().BeTrue();
    }
    
    [Fact]
    public async Task DeleteCategory_ShouldFail_WhenCategoryNotFound()
    {
        var command = new DeleteCategoryCommand(Guid.NewGuid());

        var result = await _handler.Handle(command, default);

        result.IsFailed.Should().BeTrue();
    }
    
    [Fact]
    public void DeleteCategory_ShouldReturnError_WhenIdEmpty()
    {
        var command = new DeleteCategoryCommand(Guid.Empty);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Id);
    }
}