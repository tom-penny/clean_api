namespace Shop.Application.Products.Commands;

using Common.Interfaces;
using Domain.Entities;

public class CreateProductHandler : IRequestHandler<CreateProductCommand, Result<Product>>
{
    private readonly IApplicationDbContext _context;

    public CreateProductHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Product>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var categoryIds = request.CategoryIds.Select(id => new CategoryId(id));
        
        var categories = await _context.Categories
            .Where(c => categoryIds.Contains(c.Id))
            .ToListAsync(cancellationToken);

        var product = new Product
        (
            id: new ProductId(Guid.NewGuid()),
            name: request.Name,
            stock: request.Stock,
            price: request.Price,
            categories: categories
        );
        
        _context.Products.Add(product);
        
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Ok(product);
    }
}