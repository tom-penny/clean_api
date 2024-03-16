namespace Shop.Application.Orders.Queries;

using Common.Interfaces;
using Domain.Entities;

public class GetAllOrdersHandler : IRequestHandler<GetAllOrdersQuery, Result<List<Order>>>
{
    private readonly IApplicationDbContext _context;

    public GetAllOrdersHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Result<List<Order>>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await _context.Orders.Include(o => o.Items)
            .Where(o => o.UserId == new UserId(request.UserId))
            .ToListAsync(cancellationToken);

        return Result.Ok(orders);
    }
}