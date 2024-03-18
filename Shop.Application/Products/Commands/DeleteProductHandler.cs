namespace Shop.Application.Products.Commands;

using Common.Interfaces;
using Domain.Errors;
using Domain.Entities;

public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, Result<Unit>>
{
    private readonly IApplicationDbContext _context;

    public DeleteProductHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Unit>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p =>
            p.Id == new ProductId(request.Id), cancellationToken);

        if (product == null) return Result.Fail(ProductError.NotFound(request.Id));
        
        product.Deactivate();
        
        await _context.SaveChangesAsync(cancellationToken);
        
        return Result.Ok();
    }
}