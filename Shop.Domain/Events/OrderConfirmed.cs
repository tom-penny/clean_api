namespace Shop.Domain.Events;

using Entities;
using Primitives;

public record OrderConfirmed(Order Order) : DomainEvent;