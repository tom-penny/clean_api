namespace Shop.Application.Categories.Queries;

using Interfaces;
using Domain.Entities;
using Domain.Errors;

public class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQuery, Result<Category>>
{
    private readonly IApplicationDbContext _context;

    public GetCategoryByIdHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Category>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(c =>
            c.Id == new CategoryId(request.Id), cancellationToken);

        return category != null ? Result.Ok(category) : Result.Fail(CategoryError.NotFound(request.Id));
    }
}