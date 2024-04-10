using System.Text.RegularExpressions;

namespace Shop.Domain.Entities;

using Primitives;

public record CategoryId(Guid Value);

public partial class Category : BaseEntity
{
    public CategoryId Id { get; init; }
    public string Name { get; private set; }
    public string Slug { get; private set; }

    public Category(CategoryId id, string name)
    {
        Id = id;
        Name = name;
        Slug = GenerateSlug(name);
    }

    public void Update(string name)
    {
        Name = name;
        Slug = GenerateSlug(name);
    }

    private string GenerateSlug(string name)
    {
        var slug = Regex.Replace(name, @"(?<=\s)&(?=\s)", "and");

        slug = Regex.Replace(slug, "[^0-9A-Za-z _-]", string.Empty);

        slug = slug.Replace(" ", "-");

        return slug.ToLower();
    }
    
    private Category() {}
}