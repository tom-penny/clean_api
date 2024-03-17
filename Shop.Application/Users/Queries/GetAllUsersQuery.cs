namespace Shop.Application.Users.Queries;

using Common.Models;
using Domain.Entities;

public class GetAllUsersQuery : IRequest<Result<PagedList<User>>>
{
    public required string? SortBy { get; init; }
    public required string? OrderBy { get; init; }
    public required int Page { get; init; }
    public required int Size { get; init; }
}