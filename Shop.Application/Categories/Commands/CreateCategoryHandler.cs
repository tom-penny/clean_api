namespace Shop.Application.Categories.Commands;

using Interfaces;
using Domain.Entities;
using Domain.Errors;

public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, Result<Category>>
{
    private readonly IApplicationDbContext _context;

    public CreateCategoryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Category>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var nameExists = await _context.Categories.AnyAsync(c =>
            c.Name == request.Name, cancellationToken);

        if (nameExists) return Result.Fail(CategoryError.NameExists(request.Name));

        var category = new Category
        (
            id: new CategoryId(Guid.NewGuid()),
            name: request.Name
        );
        
        _context.Categories.Add(category);
        
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Ok(category);
    }
}