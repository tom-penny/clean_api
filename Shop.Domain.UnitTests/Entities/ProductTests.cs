namespace Shop.Domain.UnitTests.Entities;

using Domain.Entities;

public class ProductTests
{
    private readonly Product _product;

    public ProductTests()
    {
        _product = new Product
        (
            id: new ProductId(Guid.NewGuid()),
            name: "product",
            stock: 10,
            price: 50m,
            new List<Category>()
        );
    }

    [Fact]
    public void Update_ShouldUpdateProductDetails()
    {
        var name = "name";
        var stock = 20;
        var price = 40m;
        
        var categories = new List<Category>
        {
            new(new CategoryId(Guid.NewGuid()), "category")
        };
        
        _product.Update(name, stock, price, categories);

        _product.Name.Should().Be(name);
        _product.Stock.Should().Be(stock);
        _product.Price.Should().Be(price);
        _product.Categories.Should().BeEquivalentTo(categories);
    }
    
    [Fact]
    public void AdjustStock_ShouldIncrementStock()
    {
        _product.AdjustStock(5);

        _product.Stock.Should().Be(15);
    }
    
    [Fact]
    public void AdjustStock_ShouldDecrementStock()
    {
        _product.AdjustStock(-5);

        _product.Stock.Should().Be(5);
    }

    [Fact]
    public void Activate_ShouldSetActiveToTrue()
    {
        _product.Activate();

        _product.IsActive.Should().BeTrue();
    }
    
    [Fact]
    public void Deactivate_ShouldSetActiveToFalse()
    {
        _product.Deactivate();

        _product.IsActive.Should().BeFalse();
    }
}