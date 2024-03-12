namespace Shop.Application.Categories.Commands;

using Interfaces;
using Domain.Entities;
using Domain.Errors;

public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, Result<Unit>>
{
    private readonly IApplicationDbContext _context;

    public DeleteCategoryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Unit>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(c =>
            c.Id == new CategoryId(request.Id), cancellationToken);

        if (category == null) return Result.Fail(CategoryError.NotFound(request.Id));

        _context.Categories.Remove(category);

        await _context.SaveChangesAsync(cancellationToken);
        
        return Result.Ok();
    }
}