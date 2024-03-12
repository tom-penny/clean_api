namespace Shop.Domain.Primitives;

using MediatR;

public record DomainEvent() : INotification;