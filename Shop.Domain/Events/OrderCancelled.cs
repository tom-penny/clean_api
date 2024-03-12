namespace Shop.Domain.Events;

using Entities;
using Primitives;

public record OrderCancelled(Order Order) : DomainEvent;