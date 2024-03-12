namespace Shop.Domain.Events;

using Entities;
using Primitives;

public record OrderProcessing(Order Order) : DomainEvent;