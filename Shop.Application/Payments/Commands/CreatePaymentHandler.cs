namespace Shop.Application.Payments.Commands;

using Common.Interfaces;
using Domain.Entities;
using Domain.Enums;

public class CreatePaymentHandler : IRequestHandler<CreatePaymentCommand, Result<Payment>>
{
    private readonly IApplicationDbContext _context;

    public CreatePaymentHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Payment>> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        var payment = new Payment
        (
            id: new PaymentId(Guid.NewGuid()),
            checkoutId: new CheckoutId(request.CheckoutId),
            amount: request.Amount,
            status: request.Status == "COMPLETED"
                ? PaymentStatus.Approved
                : PaymentStatus.Rejected
        );
        
        var order = await _context.Orders.FirstOrDefaultAsync(o =>
            o.CheckoutId == new CheckoutId(request.CheckoutId), cancellationToken);

        order?.AddPayment(payment);

        _context.Payments.Add(payment);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Ok(payment);
    }
}