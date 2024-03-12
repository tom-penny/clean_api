namespace Shop.Domain.UnitTests.Entities;

using Domain.Entities;

public class CategoryTests
{
    private readonly Category _category;

    public CategoryTests()
    {
        _category = new Category
        (
            id: new CategoryId(Guid.NewGuid()),
            name: "category"
        );
    }

    [Fact]
    public void Update_ShouldUpdateProductDetails()
    {
        var name = "name";

        _category.Update(name);

        _category.Name.Should().Be(name);
    }
}