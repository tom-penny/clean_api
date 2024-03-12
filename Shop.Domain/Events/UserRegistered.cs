namespace Shop.Domain.Events;

using Entities;
using Primitives;

public record UserRegistered(User User) : DomainEvent;