namespace Shop.Application.UnitTests.Categories.Commands;

using Domain.Entities;
using Application.Categories.Commands;

public class CreateCategoryTests
{
    private readonly TestDbContext _context;
    private readonly CreateCategoryHandler _handler;
    private readonly CreateCategoryValidator _validator;
    
    public CreateCategoryTests()
    {
        _context = new TestDbContext();
        _handler = new CreateCategoryHandler(_context);
        _validator = new CreateCategoryValidator();
    }

    [Fact]
    public async Task CreateCategory_ShouldSucceed_WhenRequestValid()
    {
        var command = new CreateCategoryCommand("category");

        var result = await _handler.Handle(command, default);

        result.IsSuccess.Should().BeTrue();
        result.Value.Name.Should().Be("category");
    }

    [Fact]
    public async Task CreateCategory_ShouldFail_WhenNameExists()
    {
        var category = new Category
        (
            id: new CategoryId(Guid.NewGuid()),
            name: "category"
        );
        
        _context.Categories.Add(category);

        await _context.SaveChangesAsync();
        
        var command = new CreateCategoryCommand("category");

        var result = await _handler.Handle(command, default);

        result.IsFailed.Should().BeTrue();
    }
    
    [Fact]
    public void CreateCategory_ShouldReturnError_WhenNameEmpty()
    {
        var command = new CreateCategoryCommand("");
        
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Name);
    }
}