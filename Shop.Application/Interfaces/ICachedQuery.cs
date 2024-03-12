namespace Shop.Application.Interfaces;

public interface ICachedQuery<out T> : IRequest<T>
{
    string Key { get; }
    TimeSpan Expiry { get; }
}