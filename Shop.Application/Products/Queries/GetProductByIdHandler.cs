namespace Shop.Application.Products.Queries;

using Common.Interfaces;
using Domain.Entities;
using Domain.Errors;

public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, Result<Product>>
{
    private readonly IApplicationDbContext _context;

    public GetProductByIdHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Product>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _context.Products.Include(p => p.Categories)
            .FirstOrDefaultAsync(p => p.Id == new ProductId(request.Id), cancellationToken);
            
        return product != null ? Result.Ok(product) : Result.Fail(ProductError.NotFound(request.Id));
    }
}