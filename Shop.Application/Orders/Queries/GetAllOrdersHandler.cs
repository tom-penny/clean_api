using System.Linq.Expressions;

namespace Shop.Application.Orders.Queries;

using Common.Models;
using Common.Interfaces;
using Common.Extensions;
using Domain.Entities;

public class GetAllOrdersHandler : IRequestHandler<GetAllOrdersQuery, Result<PagedList<Order>>>
{
    private readonly IApplicationDbContext _context;

    public GetAllOrdersHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Result<PagedList<Order>>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Orders.Where(o => o.UserId == new UserId(request.UserId));

        var sortBy = request.SortBy?.ToLower();
        var orderBy = request.OrderBy?.ToLower();

        if (sortBy != null)
        {
            query = orderBy == "desc"
                ? query.OrderByDescending(GetSortExpression(sortBy))
                : query.OrderBy(GetSortExpression(sortBy));
        }
        
        var orders = await query.Include(o => o.Items)
            .ToPagedListAsync(request.Page, request.Size, cancellationToken);

        return Result.Ok(orders);
    }
    
    private Expression<Func<Order, object>> GetSortExpression(string sortBy) => sortBy switch
    {
        "total" => order => order.Amount,
        "date" => order => order.Created,
        _ => order => order.Id
    };
}