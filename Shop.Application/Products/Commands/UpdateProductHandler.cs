namespace Shop.Application.Products.Commands;

using Common.Interfaces;
using Domain.Entities;
using Domain.Errors;

public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, Result<Unit>>
{
    private readonly IApplicationDbContext _context;

    public UpdateProductHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Unit>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p =>
            p.Id == new ProductId(request.Id), cancellationToken);

        if (product == null) return Result.Fail(ProductError.NotFound(request.Id));
        
        var categories = await _context.Categories
            .Where(c => request.CategoryIds.Contains(c.Id.Value))
            .ToListAsync(cancellationToken);

        product.Update(request.Name, request.Stock, request.Price, categories);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}