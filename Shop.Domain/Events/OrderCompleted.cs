namespace Shop.Domain.Events;

using Entities;
using Primitives;

public record OrderCompleted(Order Order) : DomainEvent;