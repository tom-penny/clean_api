namespace Shop.Domain.Entities;

using Primitives;

public record ProductId(Guid Value);

public class Product : BaseEntity
{
    public ProductId Id { get; init; }
    public string Name { get; private set; }
    public int Stock { get; private set; }
    public decimal Price { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime Created { get; init; }
    public List<Category> Categories { get; private set; }
    
    public Product(ProductId id, string name, int stock, decimal price, List<Category> categories)
    {
        Id = id;
        Name = name;
        Stock = stock;
        Price = price;
        IsActive = true;
        Created = DateTime.UtcNow;
        Categories = categories;
    }

    public void Update(string name, int stock, decimal price, List<Category> categories)
    {
        Name = name;
        Stock = stock;
        Price = price;
        Categories = categories;
    }

    public void AdjustStock(int quantity)
    {
        Stock += quantity;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    private Product() {}
}