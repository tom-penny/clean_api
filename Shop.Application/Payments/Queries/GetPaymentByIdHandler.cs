namespace Shop.Application.Payments.Queries;

using Common.Interfaces;
using Domain.Entities;
using Domain.Errors;

public class GetPaymentByIdHandler : IRequestHandler<GetPaymentByIdQuery, Result<Payment>>
{
    private readonly IApplicationDbContext _context;

    public GetPaymentByIdHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Payment>> Handle(GetPaymentByIdQuery request, CancellationToken cancellationToken)
    {
        var payment = await _context.Payments.FirstOrDefaultAsync(p =>
            p.Id == new PaymentId(request.Id), cancellationToken);
            
        return payment != null ? Result.Ok(payment) : Result.Fail(PaymentError.NotFound(request.Id));
    }
}