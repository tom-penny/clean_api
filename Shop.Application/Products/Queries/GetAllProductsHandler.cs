using System.Linq.Expressions;

namespace Shop.Application.Products.Queries;

using Domain.Entities;
using Interfaces;

public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, Result<List<Product>>>
{
    private readonly IApplicationDbContext _context;

    public GetAllProductsHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Result<List<Product>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Products.AsQueryable();

        var sortBy = request.SortBy?.ToLower();
        var orderBy = request.OrderBy?.ToLower();

        if (sortBy != null)
        {
            query = orderBy == "desc"
                ? query.OrderByDescending(GetSortExpression(sortBy))
                : query.OrderBy(GetSortExpression(sortBy));
        }

        var products = await query
            .Include(p => p.Categories)
            .Skip((request.Page - 1) * request.Size)
            .Take(request.Size)
            .ToListAsync(cancellationToken);

        return Result.Ok(products);
    }

    private Expression<Func<Product, object>> GetSortExpression(string sortBy) => sortBy switch
    {
        "name" => product => product.Name,
        "price" => product => product.Price,
        "date" => product => product.Created,
        _ => product => product.Id
    };
}