namespace Shop.Application.Orders.Commands;

using Common.Interfaces;
using Domain.Enums;
using Domain.Entities;
using Domain.Errors;

public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, Result<Order>>
{
    private readonly IApplicationDbContext _context;

    public CreateOrderHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Order>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var userExists = await _context.Users.AnyAsync(u =>
            u.Id == new UserId(request.UserId), cancellationToken);

        if (!userExists) return Result.Fail(OrderError.UserNotFound(request.UserId));
        
        var order = new Order
        (
            id: new OrderId(Guid.NewGuid()),
            checkoutId: new CheckoutId(request.CheckoutId),
            userId: new UserId(request.UserId),
            amount: request.Amount,
            status: OrderStatus.Pending
        );
        
        foreach (var item in request.Items)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p =>
                p.Id == new ProductId(item.ProductId), cancellationToken);

            if (product == null) return Result.Fail(OrderError.ProductNotFound(item.ProductId));

            if (product.Stock < item.Quantity)
            {
                return Result.Fail(OrderError.ProductOutOfStock(item.ProductId));
            }

            var orderItem = new OrderItem
            (
                id: new OrderItemId(Guid.NewGuid()),
                orderId: order.Id,
                productId: new ProductId(item.ProductId),
                unitPrice: item.UnitPrice,
                quantity: item.Quantity
            );
            
            order.Items.Add(orderItem);
        }

        var payment = await _context.Payments.FirstOrDefaultAsync(p =>
            p.CheckoutId == new CheckoutId(request.CheckoutId), cancellationToken);

        if (payment != null)
        {
            order.AddPayment(payment);
        }

        _context.Orders.Add(order);
        
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Ok(order);
    }
}