namespace Shop.Application.Categories.Queries;

using Domain.Entities;
using Interfaces;

public class GetAllCategoriesHandler : IRequestHandler<GetAllCategoriesQuery, Result<List<Category>>>
{
    private readonly IApplicationDbContext _context;

    public GetAllCategoriesHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Result<List<Category>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _context.Categories.ToListAsync(cancellationToken);

        return Result.Ok(categories);
    }
}