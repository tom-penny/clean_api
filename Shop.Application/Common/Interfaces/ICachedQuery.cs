namespace Shop.Application.Common.Interfaces;

public interface ICachedQuery<out T> : IRequest<T>
{
    string Key { get; }
    TimeSpan Expiry { get; }
}