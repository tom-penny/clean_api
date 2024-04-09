using System.Text.RegularExpressions;

namespace Shop.Domain.Entities;

using Primitives;

public record CategoryId(Guid Value);

public partial class Category : BaseEntity
{
    public CategoryId Id { get; init; }
    public string Name { get; private set; }
    public string Slug => GenerateSlug();

    public Category(CategoryId id, string name)
    {
        Id = id;
        Name = name;
    }

    public void Update(string name)
    {
        Name = name;
    }

    private string GenerateSlug()
    {
        var slug = Regex.Replace(Name, @"(?<=\s)&(?=\s)", "and");

        slug = Regex.Replace(slug, "[^0-9A-Za-z _-]", string.Empty);

        slug = slug.Replace(" ", "-");

        return slug.ToLower();
    }
    
    private Category() {}
}