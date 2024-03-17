namespace Shop.Application.Orders.Queries;

using Common.Models;
using Common.Interfaces;
using Domain.Entities;

public class GetAllOrdersQuery : IAuthorizedRequest<Result<PagedList<Order>>>
{
    public required Guid UserId { get; init; }
    public required string? SortBy { get; init; }
    public required string? OrderBy { get; init; }
    public required int Page { get; init; }
    public required int Size { get; init; }
}