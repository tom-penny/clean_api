using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Domain.Primitives;

public abstract class BaseEntity
{
    private readonly List<DomainEvent> _events = new();

    [NotMapped]
    public IReadOnlyList<DomainEvent> Events => _events.AsReadOnly();

    public void AddDomainEvent(DomainEvent domainEvent)
    {
        _events.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _events.Clear();
    }
}