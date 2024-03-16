namespace Shop.Application.Categories.Commands;

using Common.Interfaces;
using Domain.Entities;
using Domain.Errors;

public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, Result<Unit>>
{
    private readonly IApplicationDbContext _context;

    public UpdateCategoryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Unit>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(c =>
            c.Id == new CategoryId(request.Id), cancellationToken);

        if (category == null) return Result.Fail(CategoryError.NotFound(request.Id));
        
        var nameExists = await _context.Categories.AnyAsync(c =>
            c.Name == request.Name, cancellationToken);

        if (nameExists) return Result.Fail(CategoryError.NameExists(request.Name));
        
        category.Update(request.Name);
        
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}