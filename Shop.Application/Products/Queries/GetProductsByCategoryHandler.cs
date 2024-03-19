using System.Linq.Expressions;

namespace Shop.Application.Products.Queries;

using Common.Interfaces;
using Common.Models;
using Common.Extensions;
using Domain.Entities;

public class GetProductsByCategoryHandler : IRequestHandler<GetProductsByCategoryQuery, Result<PagedList<Product>>>
{
    private readonly IApplicationDbContext _context;

    public GetProductsByCategoryHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Result<PagedList<Product>>> Handle(GetProductsByCategoryQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Products.AsQueryable();

        query = Guid.TryParse(request.CategoryIdOrSlug, out var categoryId)
            ? query.Where(p => p.Categories.Any(c => c.Id == new CategoryId(categoryId)))
            : query.Where(p => p.Categories.Any(c => c.Slug == request.CategoryIdOrSlug));
        
        var sortBy = request.SortBy?.ToLower();
        var orderBy = request.OrderBy?.ToLower();

        if (sortBy != null)
        {
            query = orderBy == "desc"
                ? query.OrderByDescending(GetSortExpression(sortBy))
                : query.OrderBy(GetSortExpression(sortBy));
        }

        var products = await query.Include(p => p.Categories)
            .ToPagedListAsync(request.Page, request.Size, cancellationToken);

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