namespace Shop.Domain.Entities;

using Primitives;

public record CategoryId(Guid Value);

public class Category : BaseEntity
{
    public CategoryId Id { get; init; }
    public string Name { get; private set; }

    public Category(CategoryId id, string name)
    {
        Id = id;
        Name = name;
    }

    public void Update(string name)
    {
        Name = name;
    }
    
    private Category() {}
}