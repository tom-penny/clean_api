namespace Shop.Application.Common.Interfaces;

public interface IAuthorizedRequest<out T> : IRequest<T>
{
    Guid UserId { get; }
}