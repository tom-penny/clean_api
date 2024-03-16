namespace Shop.Application.Interfaces;

public interface IAuthorizedRequest<out T> : IRequest<T>
{
    Guid UserId { get; }
}