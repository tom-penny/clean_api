namespace Shop.Application.Orders.Queries;

using Interfaces;
using Domain.Entities;
using Domain.Errors;

public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, Result<Order>>
{
    private readonly IApplicationDbContext _context;

    public GetOrderByIdHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Order>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _context.Orders.Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == new OrderId(request.Id), cancellationToken);
            
        return order != null ? Result.Ok(order) : Result.Fail(OrderError.NotFound(request.Id));
    }
}