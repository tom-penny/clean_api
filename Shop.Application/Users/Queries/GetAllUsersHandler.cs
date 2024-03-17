using System.Linq.Expressions;

namespace Shop.Application.Users.Queries;

using Common.Models;
using Common.Interfaces;
using Common.Extensions;
using Domain.Entities;

public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, Result<PagedList<User>>>
{
    private readonly IApplicationDbContext _context;

    public GetAllUsersHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<PagedList<User>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Users.AsQueryable();

        var sortBy = request.SortBy?.ToLower();
        var orderBy = request.OrderBy?.ToLower();

        if (sortBy != null)
        {
            query = orderBy == "desc"
                ? query.OrderByDescending(GetSortExpression(sortBy))
                : query.OrderBy(GetSortExpression(sortBy));
        }
        
        var users = await _context.Users.ToPagedListAsync(request.Page, request.Size, cancellationToken);
        
        return Result.Ok(users);
    }
    
    private Expression<Func<User, object>> GetSortExpression(string sortBy) => sortBy switch
    {
        "email" => user => user.Email,
        "name" => user => user.LastName,
        "date" => user => user.Joined,
        _ => user => user.Id
    };
}