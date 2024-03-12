namespace Shop.Domain.Errors;

using Primitives;

public static class CategoryError
{
    public static DomainError NotFound(Guid id) =>
        DomainError.NotFound($"Category {id} not found");
    
    public static DomainError NameExists(string name) =>
        DomainError.Conflict($"Category {name} already exists");
}